using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainController : MonoBehaviour
    {

        public GameObject Popup1GO; // Обьект префаба первого окна
        public GameObject Popup2GO; // Обьект префаба второго окна
        public Transform PopupParent; // Трансформ канваса

        private PopupTweenModel _popup1TweenModel = new PopupTweenModel(); // Модель для твинов первого окна
        private PopupTweenModel _popup2TweenModel = new PopupTweenModel(); // Модель для твинов второго окна

        // Use this for initialization
        void Start()
        {



        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Нажатие на кнопку, показа первого окна
        /// </summary>
        public void ShowPopup1()
        {
            if (!_popup1TweenModel.CanCreateNew) // Проверка, на возможность создания нового окна
            {
                return;
            }
            if (!_popup2TweenModel.CanCreateNew) // Проверка, есть ли второе окно
            {
                _popup2HideAnim(); // Если есть спрятать
                _popup1TweenModel.CanCreateNew = false;
                return;
            }
            GameObject newPopup1 = (GameObject)Instantiate(Popup1GO, PopupParent, false); // создание обьекта
            _popup1TweenModel = new PopupTweenModel() { PopupImage = newPopup1.transform.GetChild(0).GetComponent<Image>(), PopupTransform = newPopup1.transform, CanCreateNew = false }; // заполнение модели
            newPopup1.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(_popup1HideStepOne); // вешаем онклик
            _popup1ShowAnim(); // обьект создан и настроен, показываем анимацию
        }


        /// <summary>
        /// Нажатие на кнопку, показа второго окна
        /// </summary>
        public void ShowPopup2()
        {
            // По аналогии с первым
            if (!_popup2TweenModel.CanCreateNew)
            {
                return;
            }
            if (!_popup1TweenModel.CanCreateNew)
            {
                _popup1HideStepOne();
                _popup2TweenModel.CanCreateNew = false;
                return;
            }
            GameObject newPopup2 = (GameObject)Instantiate(Popup2GO, PopupParent, false);
            _popup2TweenModel = new PopupTweenModel() { PopupImage = newPopup2.transform.GetChild(0).GetComponent<Image>(), PopupTransform = newPopup2.transform, CanCreateNew = false };
            newPopup2.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(_popup2HideAnim);
            _popup2ShowAnimStepOne();
        }


        private void _popup1ShowAnim()
        {
            _popup1TweenModel.PopupTransform.DOScale(Vector2.one, 1);
            _popup1TweenModel.PopupImage.DOFade(1, 1);
        }

        private void _popup1HideStepOne()
        {
            Tweener tween = _popup1TweenModel.PopupTransform.DOScale(new Vector2(0.5f, 0.5f), 1);
            tween.OnComplete(_popup1HideStepTwo); // ждём окончания и запускаем вторую часть анимаций

        }
        private void _popup1HideStepTwo()
        {
            Tweener tween = _popup1TweenModel.PopupTransform.DOMoveY(Screen.height + (_popup1TweenModel.PopupImage.rectTransform.sizeDelta.y / 2f * PopupParent.transform.localScale.y), 1);
            tween.OnComplete(_destroyPopupOne); // ждём окончания и удаляем обьект
        }

        /// <summary>
        /// Удаление первого окна
        /// </summary>
        private void _destroyPopupOne()
        {
            Destroy(_popup1TweenModel.PopupTransform.gameObject); // удаляем первое окно
            _popup1TweenModel.CanCreateNew = true; // разрешаем запуск первого окна заново
            if (!_popup2TweenModel.CanCreateNew) // если false, то после закрытия нужно запустить второе окно
            {
                _popup2TweenModel.CanCreateNew = true;
                ShowPopup2();
            }
        }


        // По аналогии с первым


        private void _popup2ShowAnimStepOne()
        {
            Tweener tween = _popup2TweenModel.PopupTransform.DOMoveY(Screen.height / 2f, 1);
            tween.OnComplete(_popup2ShowAnimStepTwo);
        }
        private void _popup2ShowAnimStepTwo()
        {
            _popup2TweenModel.PopupTransform.DORotate(Vector3.zero, 1);
        }

        private void _popup2HideAnim()
        {
            _popup2TweenModel.PopupTransform.DOScale(Vector2.zero, 1);
            Tweener tween = _popup2TweenModel.PopupTransform.DORotate(new Vector3(0, 0, 181), 1);
            tween.OnComplete(_destroyPopupTwo);
        }
        private void _destroyPopupTwo()
        {
            Destroy(_popup2TweenModel.PopupTransform.gameObject);
            _popup2TweenModel.CanCreateNew = true;
            if (!_popup1TweenModel.CanCreateNew)
            {
                _popup1TweenModel.CanCreateNew = true;
                ShowPopup1();
            }
        }

    }
}
public class PopupTweenModel
{
    public Transform PopupTransform; // main parent
    public Image PopupImage; //background
    public bool CanCreateNew = true; // позволяется ли создание нового
}
