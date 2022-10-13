namespace project3;


public enum TOKENTYPES {
    SEMICOLON,
    DERIVES,
    ALSODERIVES,
    EPSILON,
    SYMBOL
}

public class TOKEN {
    public TOKENTYPES token;
    public String val;

    public TOKEN (TOKENTYPES incTok, string incVal) {
        token = incTok;
        val = incVal;
    }
}