using System.Collections;

public interface IBotTask
{
    IEnumerator Execute(BotContext context);
}