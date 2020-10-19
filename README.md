# EFCore_AsyncLocal_TestApp

### Getting Started:
1. Clone the repository to your local workspace.
2. Open the `TestApp.csproj` file Visual Studio. This will load the entire project.
3. Add the details of your Postgres server [here](https://github.com/srprash/EFCore_AsyncLocal_TestApp/blob/master/Startup.cs#L33).
4. Build the project.
5. Start Debugging (F5) the project in VS.

### Reproducing the issue:
What I've tried to demonstrate in this TestApp is that using multiple `DbCommandInterceptor` instances in the EFCore pipeline to intercept db requests causes AsyncLocal to lose context.

#### Using a single DbCommandInterceptor:
1. Comment out [this](https://github.com/srprash/EFCore_AsyncLocal_TestApp/blob/master/Startup.cs#L39) line which adds `CustomInterceptor_2` to DatabaseContext.
2. Build and start debugging the application.
3. Go to `http://localhost:64116/user` in your browser.
4. In your Visual Studio, the Debug logs in Output window, you should see the following statements:
```
Inside ReaderExecutingAsync of CustomInterceptor_1
Setting AsyncLocal value as 'Value_1'
Inside ReaderExecutedAsync of CustomInterceptor_1
Getting AsyncLocalContext Value: 'Value_1'
```
You can see that AsyncLocal preserves the value across `ReaderExecutingAsync` and `ReaderExecutedAsync` methods of `CustomInterceptor_1`

#### Using two DbCommandInterceptor:
1. Uncomment [this](https://github.com/srprash/EFCore_AsyncLocal_TestApp/blob/master/Startup.cs#L39) line to add `CustomInterceptor_2` to the db request pipeline.
2. Build and start debugging the application.
3. Go to `http://localhost:64116/user` in your browser.
4. In your Visual Studio, the Debug logs in Output window, you should see the following statements:
```
Inside ReaderExecutingAsync of CustomInterceptor_1
Setting AsyncLocal value as 'Value_1'
Inside ReaderExecutingAsync of CustomInterceptor_2
Inside ReaderExecutedAsync of CustomInterceptor_1
Getting AsyncLocalContext Value: ''
Inside ReaderExecutedAsync of CustomInterceptor_2
```
Note the statement "Getting AsyncLocalContext Value" has received `null` from AsyncLocal but `Value_1` was set in the `ReaderExecutingAsync` of CustomInterceptor_1.
This means that somehow using multiple instances of DbCommandInterceptor in the EFCore call chain causes inconsistency in the AsyncLocal.
