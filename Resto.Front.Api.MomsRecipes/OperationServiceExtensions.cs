using System;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Data.Security;
using Resto.Front.Api.Exceptions;
using Resto.Front.Api.MomsRecipes.Core;
using JetBrainsNotNull = JetBrains.Annotations.NotNullAttribute;
using SystemNotNull = System.Diagnostics.CodeAnalysis.NotNullAttribute;
using JetBrains.Annotations; // Для JetBrains.Annotations.NotNullAttribute
using System.Diagnostics.CodeAnalysis; // Для System.Diagnostics.CodeAnalysis.NotNullAttribute


namespace Resto.Front.Api.MomsRecipes
{
    internal static class OperationServiceExtensions
    {        
        [NotNull]
        public static ICredentials GetCredentials([NotNull] this IOperationService operationService)
        {
            if (operationService == null)
                throw new ArgumentNullException(nameof(operationService));

            try
            {
                return operationService.AuthenticateByPin("12344321");
            }
            catch (AuthenticationException)
            {
                PluginContext.Log.Warn("Не могу пройти аутентификацию. Проверьте пин-код для пользователя плагина.");
                PluginContext.Operations.AddErrorMessage("Не могу пройти аутентификацию. Проверьте пин-код для пользователя плагина.", ExternalPaymentProcessor.paymentSystemName);
                throw;
            }
        }
    }
}
