using AiDocumentProcessing.Infrastructure.Services;
using AiDocumentProcessing.Infrastructure.Test.Common;
using FluentAssertions;
using Newtonsoft.Json;

namespace AiDocumentProcessing.Infrastructure.Test.Services;

public class AiApplicationServiceTests
{
    private const string RagData = @"
The effects of video games on adults vary with the type of game, the amount of time spent playing, and what gaming replaces in daily life.
Some studies suggest that action games can improve specific skills, such as tracking moving objects and quickly identifying relevant visual information.
These gains do not necessarily translate into better performance at work or in other everyday tasks.
Recreational gaming can help some adults relax and temporarily reduce feelings of stress.
Cooperative games can provide opportunities for teamwork and help adults stay connected with friends.
Playing late at night can delay bedtime, especially when a gaming session lasts longer than planned.
Long sessions can contribute to eye strain or neck and wrist discomfort, particularly without breaks.
Gaming can also reduce physical activity when it replaces exercise rather than other sedentary activities.
For a minority of adults, difficulty controlling gaming can interfere with work, relationships, or other responsibilities.
Time spent playing alone does not determine whether gaming is harmful; its effects on sleep, relationships, and daily functioning are more informative.
";


    [Theory]
    [InlineData("Is gaming at late night healthy or not?")]
    [InlineData("Can action games improve tracking skills?")]
    [InlineData("Are video games usefull to improve teamwork skill?")]
    public async Task evaluate_ai_response_relevance_faithfulness_correctness_completeness_to_the_context(
        string questionPrompt)
    {
        var aiConfig = ConfigurationFixture.New().GetAiConfig();
        var aiApplicationService = new AiApplicationService(aiConfig);
        var prompt = GetPrompt(RagData, questionPrompt);

        var generatedResponse = await aiApplicationService.GetResponse(prompt);

        var evaluationPrompt = GetEvaluationPrompt(RagData, questionPrompt, generatedResponse);
        var evaluationResponseInJson = await aiApplicationService.GetResponse(evaluationPrompt);
        var evaluationResult = JsonConvert.DeserializeObject<EvaluationResult>(evaluationResponseInJson);
        evaluationResult.Should().NotBeNull();
        evaluationResult.Relevance.Should().BeGreaterThanOrEqualTo(4);
        evaluationResult.Faithfulness.Should().BeGreaterThanOrEqualTo(4);
        evaluationResult.Correctness.Should().BeGreaterThanOrEqualTo(4);
        evaluationResult.Completeness.Should().BeGreaterThanOrEqualTo(4);
    }


    private static string GetPrompt(string ragData, string questionPrompt)
    {
        return @$"
            You are a question-answering assistant.

            Answer the user's question using ONLY
            the information provided in the context.

            Rules:
            1. Do not use external knowledge.
            2. Do not introduce facts, examples,
               explanations or assumptions that are
               not explicitly supported by the context.
            3. Do not make stronger claims than the
               context supports.
            4. If the context provides an opportunity
               or possibility, do not describe it as
               a proven improvement or guaranteed result.
            5. If the context does not contain enough
               information to answer the question,
               explicitly say so.
            6. Preserve important limitations and
               qualifications mentioned in the context.
            7. Keep the answer concise.

            Context:
            {ragData}

            Question:
            {questionPrompt}
            ";
    }

    private static string GetEvaluationPrompt(string ragData, string questionPrompt, string generatedResponse)
    {
        return @$"
            You are an AI response evaluator.

            Evaluate the generated answer based on the provided context.

            Context:
            {ragData}

            Question:
            {questionPrompt}

            Generated Answer:
            {generatedResponse}

            Evaluate these criteria:

            1. Relevance:
            Does the answer address the question?

            2. Faithfulness:
            Are all claims supported by the context?

            3. Correctness:
            Is the answer factually reasonable?

            4. Completeness:
            Does it cover the important information?

            Return a JSON object: {{ ""relevance"": 1, ""faithfulness"": 1, ""correctness"": 1, ""completeness"": 1, ""reason"": ""..."" }}

            Each score must be between 1 and 5.
            ";
    }


    private class EvaluationResult
    {
        public required int Relevance { get; set; }
        public required int Faithfulness { get; set; }
        public required int Correctness { get; set; }
        public required int Completeness { get; set; }
        public required string Reason { get; set; }
    }
}