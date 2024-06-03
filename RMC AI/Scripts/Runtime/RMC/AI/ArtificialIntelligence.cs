using OpenAI;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RMC.AI.Data.Types;
using RMC.Core.Interfaces;

namespace RMC.AI
{
    //  Namespace Properties ------------------------------
    
    //  Class Attributes ----------------------------------
    public class ArtificialIntelligenceUnityEvent : UnityEvent<ArtificialIntelligence> {}

    /// <summary>
    /// Replace with comments...
    /// </summary>
    public class ArtificialIntelligence : IInitializableAsync
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
        public async virtual UniTask InitializeAsync()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }

            await Task.CompletedTask;
        }


        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        


        //  Methods ---------------------------------------
      
        public async UniTask AuthenticateAsync(string pathUnderResources)
        {
            RequireIsInitialized();

            ResourceRequest resourceRequest = Resources.LoadAsync<TextAsset>(pathUnderResources);
            TextAsset textAsset = await resourceRequest.ToUniTask() as TextAsset;
            
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                ArtificialIntelligenceData artificialIntelligenceData = JsonUtility.FromJson<ArtificialIntelligenceData>(textAsset.text);
                
                if (artificialIntelligenceData.OpenAIApiKey.Length == 0)
                {
                    Debug.LogError($"Error: OpenAIApiKey is empty. See {pathUnderResources}.");
                    return;
                }
                
                if (artificialIntelligenceData.OpenAIOrganization.Length == 0)
                {
                    Debug.LogError($"Error: OpenAIOrganization is empty. See {pathUnderResources}.");
                    return;
                }
              
                OpenAISettings openAISettings = OpenAISettings.Default;
                _openAIClient = new OpenAIClient(artificialIntelligenceData.OpenAIApiKey, openAISettings);
                OnAuthenticateCompleted.Invoke(this);
            }
        }
        
        public async UniTask DeauthenticateAsync()
        {
            _openAIClient = null;
            await UniTask.CompletedTask;
        }
        
        
        //  Event Handlers --------------------------------

    }
}