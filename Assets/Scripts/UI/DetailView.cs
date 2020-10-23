using FacialExpression.AzureFaceApi;
using FacialExpression.AzureFaceApi.Models;
using FacialExpression.Helpers;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FacialExpression.UI
{
    public class DetailView : MonoBehaviour
    {
        private bool _isInfoVisible;
        
        [SerializeField] private ControlPanel controlPanel;
        [SerializeField] private EmotionDetail emotionDetail;
        [SerializeField] private RawImage image;
        [SerializeField] private ImageAnalyzer analyzer;
        [SerializeField] private RawImageSettings rawImageSettings;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;
        
        private Emotion _emotion;
        
        public void ShowView(string filePath, Texture2D texture2D)
        {
            analyzer.MakeAnalysisRequest(FileHelper.LoadImageAsArray(filePath), OnResponse);
            
            controlPanel.gameObject.SetActive(false);
            _isInfoVisible = false;
            image.texture = texture2D;
            emotionDetail.gameObject.SetActive(_isInfoVisible);
            
            aspectRatioFitter.aspectRatio = rawImageSettings.AspectRatio;
            
            image.rectTransform.localScale = new Vector3(-1f, rawImageSettings.ScaleY, 1f);
        }

        public void CloseView()
        {
            controlPanel.gameObject.SetActive(true);
            image.texture = rawImageSettings.DefaultTexture;
            gameObject.SetActive(false);
        }

        public void ShowInfo()
        {
            _isInfoVisible = !_isInfoVisible;
            emotionDetail.UpdateEmotions(_emotion);
            emotionDetail.gameObject.SetActive(_isInfoVisible);
        }

        private void OnResponse(Response response)
        {
            var faceDatas = JsonConvert.DeserializeObject<FaceData[]>(response.Data);
            _emotion = faceDatas[0].faceAttributes.emotion;
        }
    }
}