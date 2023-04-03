using UnityEngine;

namespace RMC.AI.Data.Types
{
    /// <summary>
    /// Response from Open AI
    /// </summary>
    public class ImageGenerationResponse
    {
        //  Properties -----------------------------------
        public Texture2D Texture2D { get; private set; }

        //  Fields ----------------------------------------

        //  Initialization --------------------------------

        public ImageGenerationResponse(Texture2D texture2D)
        {
            Texture2D = texture2D;
        }
        
        //  Methods --------------------------------
        public override string ToString()
        {
            return $"{this.GetType().Name}(Texture2D.width = {Texture2D.width})]";
        }
    }

}