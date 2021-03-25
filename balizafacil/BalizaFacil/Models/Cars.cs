using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BalizaFacil.Models
{
    public class Cars
    {
        public List<Car> cars { get; set; }

        public Cars()
        {
            cars = new List<Car>();
        }
        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);

        }

        public static Cars FromJSON(string json)
        {
            Cars cars = JsonConvert.DeserializeObject<Cars>(json);

            return cars;
        }

        
        public int carBrandId(string name)
        {
            return (int)(CarBrand)Enum.Parse(typeof(CarBrand), name);
        }
        public void orderByName()
        {
            cars.Sort((p, q) => string.Compare(p.Name.ToString(), q.Name.ToString()));
            //Debug.Write(cars.ToString());
        }
    }
}
