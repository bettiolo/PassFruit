using PassFruit.Ui.Wp.Views;
using PassFruit.Ui.Wp.Views.Controls;
using PassFruit.Ui.Wp.Views.Controls.Fields;

namespace PassFruit.Ui.Wp {
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Microsoft.Phone.Controls;
    using Caliburn.Micro;

    public class AppBootstrapper : PhoneBootstrapper
    {
        PhoneContainer _container;

        protected override void Configure()
        {
            _container = new PhoneContainer(RootFrame);

			_container.RegisterPhoneServices();

            _container.PerRequest<MainPageViewModel>();
            _container.PerRequest<TagsPivotPageViewModel>();
            _container.PerRequest<AccountDetailsPageViewModel>();
            _container.PerRequest<AccountProviderSelectPageViewModel>();

            _container.PerRequest<AccountProviderIconViewModel>();
            _container.PerRequest<AccountViewModel>();
            _container.PerRequest<TagViewModel>();

            _container.PerRequest<AccountFieldBase>();

            AddCustomConventions();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<Pivot>(ItemsControl.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) => {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, null, viewModelType);
                        return true;
                    }

                    return false;
                };

            ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) => {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, null, viewModelType);
                        return true;
                    }

                    return false;
                };
        }

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame() {
            return new TransitionFrame();
        }

    }
}
