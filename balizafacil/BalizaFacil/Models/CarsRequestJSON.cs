using BalizaFacil.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace BalizaFacil.Models
{
    public class CarsRequestJSON
    {
        private static CarsRequestJSON instance { get; set; }
        public static CarsRequestJSON Instance
        {
            get
            {
                if (instance is null)
                    instance = new CarsRequestJSON();

                return instance;
            }
        }


        IStorageService Storage => ServicesManager.Instance.Storage;
        
        public Cars carsDefault;
        public Cars Cars= new Cars();
        
        //public CarsRequestJSON()
        //{
        //    carsbyBrand = new List<Cars>();
        //}

        public Cars getCarsDefault(bool isBrand)
        {
            try
            {
                if (Storage.Cars == null)
                {
                    double rw = 0.57f * 0.543f; // Carro peugeot 408
                    Cars cars = new Cars();
                    /*//margem = soma 2x no tamanho da vaga e subtrai na primeira etapa           
                    //Options.Add(new Car(2, "Renault Clio H", 7,   rw,  563 + 20, -10, new ObservableCollection<double> { 205, 234, 250, 87, 154, 33 }, new ObservableCollection<double> { 175, 360, 250, 134, 100, 80 }));
                    cars.cars.Add(new Car(2, "Renault", "Clio H", 7, (rw * 225f / 250f), 567, -404, new ObservableCollection<double> { 159, 212, 237, 63, 133, 63 }, new ObservableCollection<double> { 159, 326, 237, 97, 86, 97 }));
                    cars.cars.Add(new Car(3, "Ford", "Fiesta", 7, (rw * 400f / 447f), 535, -404, new ObservableCollection<double> { 206, 180, 338, 60, 81, 63 }, new ObservableCollection<double> { 206, 257, 338, 86, 57, 71 }));
                    cars.cars.Add(new Car(4, "Renault", "Clio S", 7, rw, 563 + 20 + 54, -404, new ObservableCollection<double> { 205, 234, 250, 87, 154, 33 }, new ObservableCollection<double> { 175, 360, 250, 134, 100, 80 }));
                    // Carros com etapas a menos:
                    //Logica no Matlab -> BalizaEsquerda(1,ti,-214,ti*(7/15),ti*(8/15),0,0, 1);
                    cars.cars.Add(new Car(5, "Fiat", "Palio", 6.3, (rw * 300f / 333f), 524, 557, new ObservableCollection<double> { 250, 208, 325, 69, 98, 69, 165, 192, 277, 90, 148, 0 }, new ObservableCollection<double> { 250, 292, 325, 97, 70, 97, 165, 277, 277, 129, 103, 0 }));
                    cars.cars.Add(new Car(6, "Peugeot", "408", 8.5, rw, 625, 659, new ObservableCollection<double> { 167, 200, 260, 67, 120, 52, 148, 200, 279, 93, 157, 0 }, new ObservableCollection<double> { 167, 294, 260, 98, 82, 76, 148, 294, 279, 137, 107, 0 }));
                    cars.cars.Add(new Car(7, "Chevrolet", "Celta", 7, (rw * 300f / 338f), 519 + 15, 560, new ObservableCollection<double> { 247 - 20, 183, 332, 61, 90, 61, 164, 183, 276, 85, 143, 0 }, new ObservableCollection<double> { 247, 269, 332, 90, 61, 90, 164, 269, 276, 125, 97, 0 }));
                    cars.cars.Add(new Car(8, "Fiat", "UNO 2010", 6.5, (rw * 300f / 344f), 518, 547, new ObservableCollection<double> { 210, 184, 326, 61, 108, 48, 169, 184, 283, 86, 142, 0 }, new ObservableCollection<double> { 210, 265, 326, 88, 75, 69, 169, 265, 283, 124, 98, 0 }));
                    cars.cars.Add(new Car(9, "Fiat", "UNO Vivace 2012", 6.5, (rw * 300f / 340f), 521, 551, new ObservableCollection<double> { 191, 185, 264, 62, 110, 48, 176, 185, 283, 86, 144, 0 }, new ObservableCollection<double> { 191, 271, 264, 90, 75, 70, 176, 271, 283, 126, 99, 0 }));
                    cars.cars.Add(new Car(10, "Renault", "Logan", 7.5, (0.304655f), 575, 606, new ObservableCollection<double> { 202, 196, 259, 65, 115, 51, 185, 196, 278, 91, 151, 0 }, new ObservableCollection<double> { 202, 283, 259, 94, 80, 73, 185, 283, 278, 132, 104, 0 }));
                    //Até aqui
                    cars.cars.Add(new Car(11, "Nissan", "March", 7, (rw * 300f / 271f), 518, -404, new ObservableCollection<double> { 194, 163, 276, 54, 102, 42 }, new ObservableCollection<double> { 194, 251, 276, 84, 66, 65 }));*/
                    
                    cars.cars.Add(new Car(2, "Volkswagen", "Voyage 2017", 7, (rw * 300f / 271f), 518, -404, new ObservableCollection<double> { 11, 163, 276, 54, 102, 42 }, new ObservableCollection<double> { 11, 251, 276, 84, 66, 65 }));
                    Storage.Cars = cars;
                    
                }
                    Cars = Storage.Cars;

                
                Cars.orderByName();
                //if (isBrand)
                //    return getCarBrand(Cars);
                //else
            }catch(Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
            return Cars;

        }

        public Cars getCars(bool isBrand)
        {
            try
            {
                string url = "https://www.balizafacil.com.br/carros-por-marca.json";
                var json = new WebClient().DownloadString(url);
                Cars cars = JsonConvert.DeserializeObject<Cars>(json);
                Car carAux = null;

                if (Storage.Cars.cars.Count > cars.cars.Count)
                {
                    //int cont = Storage.Cars.cars.Count - cars.cars.Count;
                    //for(int c = 0;c < cont;c++)
                        carAux = Storage.Cars.cars[Storage.Cars.cars.Count - 1];
                }
                    
                if (!cars.Equals(carsDefault))
                {
                    carsDefault = cars;
                    Storage.Cars = carsDefault;
                }
                if (carAux != null)
                    cars.cars.Add(carAux);
                cars.orderByName();
                Cars = cars;
                return cars;

            }
            catch
            {
                    return getCarsDefault(false);

            }
        }


        public Cars updateCars(Cars old)
        {
            Cars cars = new Cars();
            foreach (Car car in Storage.Cars.cars)
            {
                
            }

            return cars;
        }

        public Cars searchCars(string style)
        {
            Cars cars = new Cars();
            foreach (Car car in Cars.cars)
            {
                if (car.Style.ToLower().Contains(style.ToLower()))
                {
                    cars.cars.Add(car);
                }

            }
            return cars;
        }



        public bool getUpdate()
        {
            Car old = Storage.Car;
            if (old != null)
            {
                Cars cars = getCars(false);
                foreach (Car carro in cars.cars)
                {
                    if (carro.Id == old.Id && carro.Style.Equals(old.Style))
                        return !carro.compareCars(old);
                }
            }
            return false;
        }

        public void updateCar()
        {
            Car old = Storage.Car;
            if (old != null)
            {
                Cars cars = getCars(false);
                foreach (Car carro in cars.cars)
                {
                    if (carro.Id == old.Id && carro.Style.Equals(old.Style))
                        old = carro;
                }
                Storage.Car = old;
                BalizaFacil.App.Instance.Display("Atualizado", "carro atualizado");
            }
        }
    }
}
