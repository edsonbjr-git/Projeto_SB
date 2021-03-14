using System;
using System.Collections.Generic;
using BalizaFacil.Models;

[assembly: Xamarin.Forms.Dependency(typeof(BalizaFacil.Services.DataStoreContainer))]
namespace BalizaFacil.Services
{
    public class DataStoreContainer
    {
        private FirebaseAuthService _firebaseAuthService;
        private IDataStore<LogBaliza> _postStore;

        public DataStoreContainer(FirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }

        public IDataStore<LogBaliza> LogBalizaStore
        {
            get
            {
                if (_postStore == null)
                {
                    _postStore = new FirebaseDataStore<LogBaliza>(_firebaseAuthService, "balizas");
                }

                return _postStore;
            }
        }

        
    }
}