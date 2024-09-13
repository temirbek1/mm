using API.MomsRecipes;
using Newtonsoft.Json;
using Resto.Front.Api.Attributes;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Data.Organization;
using Resto.Front.Api.Data.View;
using Resto.Front.Api.Exceptions;
using Resto.Front.Api.MomsRecipes.Core;
using Resto.Front.Api.MomsRecipes.Core.Logging;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;
using Resto.Front.Api.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using JetBrainsNotNull = JetBrains.Annotations.NotNullAttribute;
using SystemNotNull = System.Diagnostics.CodeAnalysis.NotNullAttribute;
using JetBrains.Annotations; // Для JetBrains.Annotations.NotNullAttribute
using System.Diagnostics.CodeAnalysis; // Для System.Diagnostics.CodeAnalysis.NotNullAttribute


namespace Resto.Front.Api.MomsRecipes
{
    [UsedImplicitly]
    [PluginLicenseModuleId(21016318)]
    public sealed class MomsRecipesPlugin : MarshalByRefObject, IFrontPlugin, IDisposable
    {
        public static CompositeDisposable subscriptions;
        private IPluginIntegrationService _pluginIntegrationService;
        public static ICurrencySettings CurrencySettings;
        public static ILogService _MomsRecipesLog = new PluginContextLogService();
        ExternalPaymentProcessor paymentSystem;


        private readonly APIWorker MomsRecipesAPI;

        public void DisposeAsync()
        {
            if (subscriptions != null)
                subscriptions.Dispose();
        }

        public void Dispose()
        {
            paymentSystem.Dispose();
        }
        public void Init(IServiceProvider serviceProvider)
        {
            

        }
        public MomsRecipesPlugin()
        {
            if (CoreObject.Instance.Configuration.Config.DebugMode)
                Debugger.Launch();
            APIWorker.GetInstance(_MomsRecipesLog);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            _MomsRecipesLog.Info($"Версия плагина - {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            String fileName = Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "Resto.Front.Api.MomsRecipes", "Resto.Front.Api.MomsRecipes.dll");
            if (File.Exists(fileName))
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
                _MomsRecipesLog.Info($"Версия модуля(Api.MomsRecipes) - {myFileVersionInfo.FileVersion}");
            }

            var operationService = (IOperationService)PluginContext.Operations;

            paymentSystem = new ExternalPaymentProcessor(APIWorker.GetInstance(), operationService);

            _MomsRecipesLog.Info("DepartmentId - " + PluginContext.Operations.GetHostRestaurant().DepartmentId);
            _MomsRecipesLog.Info("CompanyName - " + PluginContext.Operations.GetHostRestaurant().CompanyName);
            try
            {
                var credentials = PluginContext.Operations.GetCredentials();
                if (credentials.Id == Guid.Empty)
                {
                    PluginContext.Operations.AddErrorMessage("Не могу пройти аутентификацию. Проверьте пин-код для пользователя плагина.", ExternalPaymentProcessor.paymentSystemName);
                }
            }
            catch
            {
                return;
            }
            CurrencySettings = PluginContext.Operations.GetHostRestaurant().Currency;
            _MomsRecipesLog.Info($"Настройки округления - {CurrencySettings.MinimumDenomination.ToString()}");
            _MomsRecipesLog.Info($"GetPaymentTypes:");
            foreach (var d in PluginContext.Operations.GetPaymentTypes())
            {
                _MomsRecipesLog.InfoFormat($"====>{d.Id} - {d.Name} - {d.DiscountType?.Name} - {d.DiscountType?.DiscountByFlexibleSum} - {d.Name.ToLower().Contains(paymentSystem.PaymentSystemKey.ToLower())}");
            }
            _MomsRecipesLog.Info($"GetDiscountTypes:");
            foreach (var data in PluginContext.Operations.GetDiscountTypes())
            {
                _MomsRecipesLog.InfoFormat($"====>{data.Id} - {data.Name}");
            };

            try
            {
                subscriptions.Add(PluginContext.Operations.RegisterPaymentSystem(paymentSystem, false));
            }
            catch (LicenseRestrictionException ex)
            {
                _MomsRecipesLog.Warn(ex.Message);
                return;
            }


            _MomsRecipesLog.InfoFormat("Payment system '{0}': '{1}' was successfully registered on server.", paymentSystem.PaymentSystemKey, paymentSystem.PaymentSystemName);
            _pluginIntegrationService = (IPluginIntegrationService)PluginContext.Services.GetService(typeof(IPluginIntegrationService));
            //_pluginIntegrationService.AddButton(new Button("MomsRecipes", StartSync));
            //_pluginIntegrationService.AddButtonOnPastOrderView(new ButtonOnPastOrderView("asdfasdf", null));            
            //InitContext(PluginEnvironment.MainControllerExists);
        }


        private void StartSync(IViewManager viewManager, IReceiptPrinter receiptPrinter)
        {            
        }
    }
}
