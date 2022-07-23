using Microsoft.ML;
using Sentiment.Domain.Entities;

namespace Sentiment.Domain.Services
{
    public class SentimentalService
    {
        private ITransformer model;
        private MLContext context = new MLContext();

        public SentimentalService()
        {
            DataViewSchema modelSchema;
         
            using (var stream = System.IO.File.OpenRead(@"model.zip"))
            {
                model = context.Model.Load(stream, out modelSchema);
            }
        }

        public bool GetSentimantal(string comment)
        {
            PredictionEngine<SentimentData, SentimentPrediction> predictionFunction = context.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
            SentimentData sampleStatement = new SentimentData
            {
                SentimentText = comment
            };

            var resultPrediction = predictionFunction.Predict(sampleStatement);

            return Convert.ToBoolean(resultPrediction.Prediction) ? false : true;
        }
    }
}
