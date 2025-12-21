using System.Collections;

public interface IBotTask
{
    IEnumerator Execute(BotContext context, float timeDelay = 0.5f);
}