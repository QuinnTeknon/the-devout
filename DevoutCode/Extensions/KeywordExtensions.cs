using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Devout.DevoutCode.Extensions;

public static class KeywordExtensions {
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Unsheathed;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Enthralling;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Temporary;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Rhythmic;

}
