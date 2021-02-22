### What is it?
Library to integrate .net core app with Grafana Loki.

# How to run it? 

Just run the file called docker-compose.yaml :

```
version: '3.7'
services:
  loki:
    image: grafana/loki:2.0.0
    ports:
      - "3100:3100"
    command: -config.file=/etc/loki/local-config.yaml
    networks:
      - loki
  grafana:
    image: grafana/grafana:latest
    ports:
      - "3000:3000"
    networks:
      - loki
networks:
  loki:
    name: loki
```
Then run in terminal
```
docker-compose up -d
```
Next step is add configuration in appSettings.json

```
"SerilogLoki": {
    "User": "admin",
    "Password" : "admin",
    "Address" : "http://localhost:3100"
  }
```

And then add method call in program.cs : 

```
private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                    {
                        ...
                    }
                )
                .Configure(app =>
                    {
                        ...
                    }
                )
                .UseSerilogLoki()
        ;
```
And thats all! 
