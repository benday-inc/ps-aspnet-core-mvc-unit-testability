﻿using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.Api.Models;

public static class PersonExtensionMethods
{
    public static PersonFact GetFact(
        this IList<PersonFact> facts,
        string factType)
    {
        var returnValue = (
            from temp in facts
            where temp.FactType == factType
            select temp
            ).FirstOrDefault();

        return returnValue;
    }

    public static IList<PersonFact> GetFacts(
        this IList<PersonFact> facts,
        string factType)
    {
        var returnValue = (
            from temp in facts
            where temp.FactType == factType
            select temp
            ).ToList();

        return returnValue;
    }

    // GetFactValueAsString

    public static string GetFactValueAsString(
        this IList<PersonFact> facts,
        string factType)
    {
        var temp = facts.GetFact(factType);

        if (temp == null)
        {
            return null;
        }
        else
        {
            return temp.FactValue;
        }
    }

    public static DateTime GetFactValueAsDateTime(
        this IList<PersonFact> facts,
        string factType)
    {
        var temp = facts.GetFact(factType);

        if (temp == null)
        {
            return default(DateTime);
        }
        else
        {
            return temp.StartDate;
        }
    }
}
