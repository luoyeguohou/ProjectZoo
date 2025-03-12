using UnityEngine;
public static class CardUtil
{
    public static Card GenCardByUid(string uid) {
        switch (uid) {
            case "monkey":
                return new Monkey();
            case "elephent":
                return new Elephent();
        }
        return new Card();
    }
}

public class Monkey : Card
{
    public Monkey() {
        uid = "monkey";
        cfg = Cfg.cards[uid];
        url = "ui://Main/testCard";
    }
}

public class Elephent : Card
{
    public Elephent()
    {
        uid = "elephent";
        cfg = Cfg.cards[uid];
        url = "ui://Main/testCard";
    }
}