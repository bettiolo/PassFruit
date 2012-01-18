using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client {
    
    public class Repositories {

        private readonly IList<IRepositoryInfo> _repositories = new List<IRepositoryInfo>();

        private IRepositoryInfo _currentRepository;

        public void AddRepository(IRepositoryInfo repository) {
            _repositories.Add(repository);
        }

        public IList<IRepositoryInfo> GetAvailableRepositories() {
            return _repositories;
        }

        public void SelectRepository(IRepositoryInfo repository) {
            _currentRepository = repository;
        }

        public IRepositoryInfo GetSelectedRepository() {
            return _currentRepository;
        }



    }
}
