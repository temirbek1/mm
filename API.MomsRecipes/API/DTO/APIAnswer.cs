using System.ComponentModel;
using System.Net;

namespace API
{
    public enum APIAnswer : int
    {
        [Description("Хорошо")]
        OkRequest = HttpStatusCode.OK,
        [Description("Создано")]
        CreatedRequest = 201,
        [Description("Запрос принят")]
        Accepted = 202,
        [Description("Объект не найден, некорректный путь")]
        BadRequest = 404,
        [Description("Доступ запрещён (авторизован, но нет прав)")]
        AccessDenied = 403,
        [Description("Конфликт")]
        Conflict = 409,
        [Description("Требуется оплата API (отсутствует лицензия)")]
        ApiNotPaid = 402,
        [Description("Другие ошибки")]
        OtherError = 500,
    }


}
