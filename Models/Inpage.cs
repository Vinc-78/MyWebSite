namespace MyWebSite.Models
{
    public class Inpage
    {
        public int TotalItems { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; } //quanti elementi per pagina

        public int TotalPages { get; private set; }

        public int StartPAge{ get; private set; }

        public int EndPage { get; private set; }

        public Inpage()
        { }

        public Inpage(int totalItems, int page, int pageSize = 10)
        { 
            int totalPage = (int)Math.Ceiling((decimal)totalItems/ (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if ( startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPage)
            {
                endPage = totalPage;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            this.TotalItems = totalItems;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalPages = totalPage;
            this.StartPAge = startPage;
            this.EndPage = endPage;

        }
    }

    

}
