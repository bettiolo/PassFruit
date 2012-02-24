using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client {
    
    public class Repositories {

        private readonly IList<IRepository> _repositories = new List<IRepository>();

        private IRepository _currentRepository;

        public void AddRepository(IRepository repository) {
            _repositories.Add(repository);
        }

        public IList<IRepository> GetAvailableRepositories() {
            return _repositories;
        }

        public void SelectRepository(IRepository repository) {
            _currentRepository = repository;
        }

        public IRepository GetSelectedRepository() {
            return _currentRepository;
        }

    }

}
