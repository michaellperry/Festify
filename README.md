# Festify Example Application

Example app for the course "Fundamentals of Distributed Systems" on Pluralsight, by Michael L Perry.

## Setting Up a Mac

To run on a Mac, you will need to run SQL Server in a Docker container.
Install Docker Desktop for the Mac, and then pull the base image using `Scripts/pull.sh`.
Then start up a container using `Scripts/startsql.sh`.

To connect to that instance of SQL Server, you will need to change the connection string.
The easiest way to do this is with User Secrets.
Manage the user secrets for Festify.Promotion.csproj.
You can use the [.NET Core User Secrets](https://marketplace.visualstudio.com/items?itemName=adrianwilczynski.user-secrets) extension by Adrian Wilczyński.
Set your user secrets file like this:

```json
{
    "ConnectionStrings": {
        "PromotionContext": "Data Source=.;Database=Festify-promotion;User ID=sa;Password=Pass@word1;MultipleActiveResultSets=true"
    }
}
```

Alternatively, you can use the dotnet CLI to set the user secret.
Change to the Festify.Promotion folder and run this command:

```bash
dotnet user-secrets set "ConnectionStrings:PromotionContext" "Data Source=.;Database=Festify-promotion;User ID=sa;Password=Pass@word1;MultipleActiveResultSets=true"
```

## Creating the Database

Install the EF command-line tools in order to work with the application database.
Run this command:

```bash
dotnet tool install --global dotnet-ef
```

Initialize the application database by running migrations.
Use the following command:

```bash
dotnet ef database update --project Festify.Promotion/
```

## Running the App

Start up the Promotion Web application with this command:

```bash
dotnet run --project Festify.Promotion
```

Or run Festify.Promotion from Visual Studio.

## Running the Emailer

The Emailer is a mock service that stands in for a process that emails about new shows.
It uses MassTransit to manage RabbitMQ.
To start RabbitMQ, create a Docker container.
To start it in a Docker container, run the shell script:

```bash
Scripts/startrabbitmq.sh
```

Then start the Emailer and schedule a show.

## Running the Indexer

The Indexer also requires RabbitMQ.
Follow the instructions above for the Emailer.
In addition, the Indexer requires Elasticsearch.
To start it in a Docker container, run the shell script:

```bash
Scripts/startelasticsearch.sh
```

Visit [http://localhost:9200](http://localhost:9200) in your browser to verify that it is running.
Then schedule a show and query Elasticsearch at [http://localhost:9200/shows/_search?pretty](http://localhost:9200/shows/_search?pretty).
