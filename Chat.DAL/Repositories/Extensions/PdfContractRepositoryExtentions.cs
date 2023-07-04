using Chat.DAL.Entities;

namespace Chat.DAL.Repositories.Extensions
{
    public static class PdfContractRepositoryExtentions
    {
        public static IQueryable<PdfContract> SearchInContractsInfo(this IQueryable<PdfContract> contracts, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return contracts;

            var lowerCaseSearchText = searchText.Trim().ToLower();
            var searchResult = contracts;
            return searchResult;
        }
    }
}
