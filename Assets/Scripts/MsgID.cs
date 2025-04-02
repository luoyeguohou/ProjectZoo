using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MsgID
{
    // card
    ActionDrawCard,
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
    ActionDeleteBadIdea,
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
    // zoo land
    ActionClearRock,
    ActionClearLake,
    // zoo
    ActionPlayAHandFreely,
    ActionBuildBigVenueFreely,
    ActionBuildMonkeyVenue,
    ActionDemolitionVenueWithCost,
    ActionExpand,
    ActionExpandFreely,
    ActionDemolitionVenue,
    // shop
    ActionGoShop,
    // internal msg
    ResolveEventChoiceEffect,
    ResolveCardEffect,
    ResolveWorkPosEffect,
    ResolveEndSeason,
    ResolveNextEvent,
    UseWorker,
    AddVenue,
    RemoveVenue,
    // used to update view
    AfterMapChanged,
    AfterBookChanged,
    AfterWorkPosChanged,
    AfterWorkerChanged,
    AfterPopRatingChanged,
    AfterGoldChanged,
    AfterTimeResChanged,
    AfterCardChanged,
    // outside the game
    StartGame,
}
