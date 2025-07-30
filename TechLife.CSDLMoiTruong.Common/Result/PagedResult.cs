using System;
using System.Collections.Generic;
using System.Text;

namespace TechLife.CSDLMoiTruong.Common.Result
{
    public class PagingRequestBase
    {
        public int PageIndex { get; set; } = SystemConstants.pageIndex;

        public int PageSize { get; set; } = SystemConstants.pageSize;

    }

    public class PagedResultBase: PagingRequestBase
    {
        public int TotalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
    public class GetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; } = string.Empty;

    }
    public class RequestBase 
    {
        public Guid UserId { get; set; }

    }
    public class UpdateRequestBase
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }

    }
    public class DeleteRequest : UpdateRequestBase
    {
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Action { get; set; }
        public string ViewCallBack { get; set; }
    }
    public class UpdateOrderRequest : UpdateRequestBase
    {
        public int Value { get; set; }
    }
    public class UpdateStringValueRequest : UpdateRequestBase
    {
        public string Value { get; set; }
    }
    public class UpdateStatusRequest : UpdateRequestBase
    {
    }
}
