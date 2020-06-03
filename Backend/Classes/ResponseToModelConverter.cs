using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Newtonsoft.Json;

namespace Backend.Classes
{
    //LATER: if i have one converter i have to have converter for every api call?
    //Rename to itemModelConverter?
    public static class ResponseToModelConverter
    {
        public static List<ItemModel.RootObject> ConvertAllResponses(List<Task<string>> responses)
        {
            List<ItemModel.RootObject> Models = new List<ItemModel.RootObject>();
            for (int responseNumber = 0; responseNumber < responses.Count; responseNumber++)
            {
                Models.Add(ConvertOneResponce(responses[responseNumber]));
            }
            return Models;
        }

        public static ItemModel.RootObject ConvertOneResponce(Task<string> response)
        {
            return JsonConvert.DeserializeObject<ItemModel.RootObject>(response.Result);
        }
    }
}
