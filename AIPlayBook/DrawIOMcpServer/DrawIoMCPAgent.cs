using Azure.AI.OpenAI;
using Common;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI.Chat;
using System.ClientModel;

namespace DrawIOMcpServer
{
    public class DrawIoMCPAgent
    {
        public static async Task RunAsync(Configuration configuration)
        {
            AzureOpenAIClient client = new(new Uri(configuration.AzureOpenAIEndpoint),
                new ApiKeyCredential(configuration.AzureOpenAIApiKey));

            await using var drawIoMcpClient = await McpClient.CreateAsync(
              new StdioClientTransport(new()
              {
                  Name = "Draw.IO MCP Server",
                  Command = "npx",
                  Arguments = ["-y", "drawio-mcp-server", "--editor", "--http-port", "4000"]
              }));

            var mcpTools = await drawIoMcpClient.ListToolsAsync().ConfigureAwait(false);

            AIAgent agent = client
                .GetChatClient("gpt-4o")
                .AsAIAgent(
                    instructions: "You are a Draw.IO MCP Server which can draw diagrams based on user prompts. Always apply colors to the shapes. ",
                    tools: [.. mcpTools.Cast<AITool>()]
                )
                .AsBuilder()
                .Build();

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if(input is null)
                {
                    break;
                }
                AgentResponse response = await agent.RunAsync(input);

                Console.WriteLine(response);
                Utils.Separator();
            }
        }
    }
}
