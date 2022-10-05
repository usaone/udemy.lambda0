using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace udemy.lambda;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<User> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        request.PathParameters.TryGetValue("userId", out var userIdString);
        Guid.TryParse(userIdString, out var userId);

        var dynamoDBContext = new DynamoDBContext(new AmazonDynamoDBClient());
        var user = await dynamoDBContext.LoadAsync<User>(userId);

        return user;
    }

    /// <summary>
    /// A simple function that takes Temperature and Converts Celcius to Fahrenheit and Fahrenheit to Celcius
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Temperature C2FandF2C(Temperature input, ILambdaContext context)
    {
        var tempval = input.Value;
        var temperature = new Temperature();
        if (input.CorF == null)
            LambdaLogger.Log("Assuming input in Fahrenheit and hence converting to Celcius");
        if (input.CorF == "C")
        {
            temperature.Value = (tempval * 9.0 / 5.0) + 32.0;
            temperature.CorF = "F";
        }
        else
        {
            temperature.Value = (tempval - 32.0) * 5.0 / 9.0;
            temperature.CorF = "C";
        }

        return temperature;
    }

}

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}