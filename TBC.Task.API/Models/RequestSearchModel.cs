namespace TBC.Task.API.Models;

public record RequestSearchModel(string Keyword, DateTime? BirthDateFrom, DateTime? BirthDateTo, int CurrentPage, int PageSize)
    : RequestQuickSearchModel(Keyword, CurrentPage, PageSize);
