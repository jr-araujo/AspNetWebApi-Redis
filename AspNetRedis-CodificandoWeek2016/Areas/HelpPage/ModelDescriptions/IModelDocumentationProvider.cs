using System;
using System.Reflection;

namespace AspNetRedis_CodificandoWeek2016.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}