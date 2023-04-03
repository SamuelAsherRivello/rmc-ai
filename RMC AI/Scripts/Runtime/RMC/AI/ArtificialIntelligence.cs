using OpenAI;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;
using RMC.AI.data.types;

namespace RMC.AI
{
    //  Namespace Properties ------------------------------
    
    //  Class Attributes ----------------------------------
    public class ArtificialIntelligenceUnityEvent : UnityEvent<ArtificialIntelligence> {}

    /// <summary>
    /// Replace with comments...
    /// </summary>
    public class ArtificialIntelligence 
    {
        //  Events ----------------------------------------
        public ArtificialIntelligenceUnityEvent OnAuthenticateCompleted = new ArtificialIntelligenceUnityEvent();

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }

        public OpenAIClient OpenAIClient
        {
            get
            {
                RequireIsInitialized();
                return _openAIClient;
            }
        }
        
        public bool IsAuthenticated
        {
            get
            {
                //Don't wait for init
                return _openAIClient != null;
            }
        }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private OpenAIClient _openAIClient;

        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        


        //  Methods ---------------------------------------
        public async UniTask AuthenticateAsync()
        {
            await AuthenticateAsync("ArtificialIntelligenceData");
        }

        public async UniTask AuthenticateAsync(string fileName)
        {
            RequireIsInitialized();

            ResourceRequest resourceRequest = Resources.LoadAsync<TextAsset>(fileName);
            TextAsset textAsset = await resourceRequest.ToUniTask() as TextAsset;
            
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                ArtificialIntelligenceData artificialIntelligenceData = JsonUtility.FromJson<ArtificialIntelligenceData>(textAsset.text);
                _openAIClient = new OpenAIClient(artificialIntelligenceData.OpenAIApiKey);
                OnAuthenticateCompleted.Invoke(this);
            }
        }
        
        
        //  Event Handlers --------------------------------

    }
}