using OpenAI.Chat;
using System;
using System.Threading.Tasks;

public class AIChatService
{
    private readonly ChatClient _chatClient;

    public AIChatService()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") 
            ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set");
            
        _chatClient = new ChatClient(model: "gpt-4o", apiKey: apiKey);
    }

    public async Task<string> GetCodeCompletionAsync(string prompt, EditorContext context)
    {
        try
        {
            var systemMessage = "You are a code completion AI. " +
                               "Respond ONLY with code that will replace the selected text. " +
                               "No explanations, no markdown, just pure code that can be directly inserted. " +
                               "Consider the full file context when generating completions.";

            var contextInfo = $"File: {context.CurrentFile}\n" +
                             $"Full File Content:\n{context.CurrentContent}\n\n" +
                             $"Selected Text (to be replaced):\n{context.SelectedText}\n" +
                             $"Selection Position: Offset {context.SelectionStart}, Length {context.SelectionLength}";

            var completion = await _chatClient.CompleteChatAsync($"{systemMessage}\n\nContext:\n{contextInfo}\n\nRequest: {prompt}");
            return completion.Value.Content[0].Text.Trim();
        }
        catch (Exception ex)
        {
            return $"// Error: {ex.Message}";
        }
    }

    public async Task<string> GetResponseAsync(string message, EditorContext context)
    {
        try
        {
            var systemMessage = "You are an intelligent programming assistant. " +
                                "You have access to the current file and project context. " +
                                "Provide clear, concise responses and code examples when appropriate.";

            var contextInfo = $"Current file: {context.CurrentFile}\n" +
                              $"Current content: {context.CurrentContent}\n" +
                              $"Project files: {string.Join(", ", context.ProjectFiles)}";

            var prompt = $"{systemMessage}\n\nContext:\n{contextInfo}\n\nUser Question: {message}";

            var completion = await _chatClient.CompleteChatAsync(prompt);
            return completion.Value.Content[0].Text.Trim();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public async Task StreamCodeCompletionAsync(string prompt, EditorContext context, Action<string> onToken)
    {
        try
        {
            var systemMessage = "You are a code completion AI. " +
                               "Respond ONLY with code that will replace the selected text. " +
                               "No explanations, no markdown, just pure code that can be directly inserted. " +
                               "Consider the full file context when generating completions.";

            var contextInfo = $"File: {context.CurrentFile}\n" +
                             $"Full File Content:\n{context.CurrentContent}\n\n" +
                             $"Selected Text (to be replaced):\n{context.SelectedText}\n" +
                             $"Selection Position: Offset {context.SelectionStart}, Length {context.SelectionLength}";

            var completionUpdates = _chatClient.CompleteChatStreamingAsync($"{systemMessage}\n\nContext:\n{contextInfo}\n\nRequest: {prompt}");
            
            await foreach (var update in completionUpdates)
            {
                if (update.ContentUpdate.Count > 0)
                {
                    onToken(update.ContentUpdate[0].Text);
                }
            }
        }
        catch (Exception ex)
        {
            onToken($"// Error: {ex.Message}");
        }
    }
} 