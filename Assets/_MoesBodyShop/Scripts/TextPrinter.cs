﻿namespace Graveyard
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    public class TextPrinter : MonoBehaviour
    {

        public delegate void OnPrintFinish();
        public event OnPrintFinish PrintFinish;

        [SerializeField] private GameObject textObject;
        [SerializeField] private float characterSpeed = 0.2f;
        [SerializeField] private float textCloseDelay = 1f;

        private Coroutine printCoroutine;
        private Text text;
        private string content;

        private void Awake()
        {
            text = GetComponent<Text>();
            textObject.SetActive(false);
        }

        public void SetText(string newContent)
        {
            content = newContent;

            textObject.SetActive(true);
            if (printCoroutine != null)
            {
                StopCoroutine(printCoroutine);
            }

            printCoroutine = StartCoroutine(TextPrint(newContent));
        }

        public void ForceFinish()
        {
            if (printCoroutine != null)
            {
                StopCoroutine(printCoroutine);
                printCoroutine = null;

                text.text = content;

                if (PrintFinish != null)
                {
                    PrintFinish.Invoke();
                }
            }
        }

        private IEnumerator TextPrint(string newContent)
        {
            //Clear content
            text.text = string.Empty;

            for (int i = 0; i < newContent.Length; i++)
            {
                if (newContent[i] == '/')
                {
                    text.text += "\n";
                }
                else
                {
                    text.text += newContent[i];
                }

                yield return new WaitForSeconds(characterSpeed);
            }

            if (PrintFinish != null)
            {
                PrintFinish.Invoke();
            }

            yield return new WaitForSeconds(textCloseDelay);
            textObject.SetActive(false);
        }
    }
}