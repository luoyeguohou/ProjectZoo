public enum MsgID
{
    // card
    ActionDrawCardAndMayDiscard,
    ActionRecycleCard,
    ActionDiscardCardFromDrawPile,
    ActionDiscardCardAndDrawSame,
    ActionDiscardCardAndDrawSameWithLimit,
    ActionDiscardCardAndGainGold,
    ActionCopyCard,
    ActionGainRandomDepCard,
    ActionGainRandomBadIdeaCard,
    ActionGainLastProjectCard,
    ActionGainSpecificCard,
    ActionCopyCardFromVegue,
    ActionTryToPlayHand,
    ActionAddHandLimit,
    ActionGainSpecTypeCard,
    ActionDeleteBadIdeaCard,
    // buff
    ActionBuffChanged,
    // gold
    ActionGainIncome,
    ActionDoubleGold,
    ActionGainGold,
    ActionPayGold,
    // map bonus
    ActionGainMapBonus5Gold,
    ActionGainRandomMapBonus,
    // popR
    ActionGainPopR,
    ActionGainVenuePopR,
    // time
    ActionGainTime,
    ActionPayTime,
    // workPos
    ActionTraining,
    ActionTrainingPromotionDep,
    ActionGainWorkPos,
    // worker
    ActionGainWorker,
    ActionGainTWorker,
    ActionGainSpecWorker,
    // book
    ActionGainRandomBook,
    ActionUseBook,
    ActionGainBook,
    ActionDiscardBook,
    ActionSellBook,
    // zoo land
    ActionClearRock,
    ActionClearLake,
    ActionGainMapBonus,
    // zoo
    ActionPlayAHandFreely,
    ActionBuildBigVenueFreely,
    ActionBuildMonkeyVenue,
    ActionDemolitionVenueWithCost,
    ActionExpand,
    ActionExpandRandomly,
    ActionDemolitionVenue,
    // shop
    ActionGoShop,
    ActionBuyBook,
    ActionBuyCard,
    ActionDiscardCardInShop,
    // internal msg
    ResolveEventChoiceEffect,
    ResolveCardEffect,
    ResolveCardsEffect,
    ResolveWorkPosEffect,
    ResolveEndSeason,
    ResolveStartSeason,
    ResolveEvent,
    UseWorker,
    AddVenue,
    RemoveVenue,
    // card inner
    DiscardCard,
    CardFromDrawToHand,
    CardFromDrawToDiscard,
    CardToHand,
    CardFromHandToDiscard,
    CardFromDiscardToHand,
    DeleteCardFromHand,
    // used to update view or trigger passive
    AfterGainCard,
    AfterMapChanged,
    AfterBookChanged,
    AfterWorkPosChanged,
    AfterWorkerChanged,
    AfterPopRatingChanged,
    AfterGoldChanged,
    AfterTimeResChanged,
    AfterCardChanged,
    AfterShopChanged,
    AfterConsoleChanged,
    AfterTurnChanged,
    AfterBuffChanged,
    // outside the game
    StartGame,
    // Console
    ConsoleMsg,
    // Ani
    VenueTakeEffectAni,
}
