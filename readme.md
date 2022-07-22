# Prompt

1. Implement a C# Web API using .NET or .NET Core using the requirements listed below.

2. Showcase the following:

   a. Usage of Dependency Injection

   - [Program](api\Program.cs)
   - [Controllers](api\Controllers)
   - [Services](api\Services)

   b. Generics, extension methods

   - Generics [IFibService.cs](api\Services\IFibService.cs)

   c. Async programming

   - [Stress Test](stress\Program.cs)
   - [Controllers](api\Controllers)

   d. Multi-threading

   - AsParallel [ListFibWithCache](api\Services\IntFibService.cs)
   - Did NOT complete BackgroundTaskQueue

   e. Exceptions handling

   - [Controllers](api\Controllers)
   - [Services](api\Services)
   - [IntExtension](api\Extensions\IntExtension.cs)

   f. JSON serialization/deserialization

   - [Logs](api\Services\AppMonitorService.cs)

3. Benefits:

   a. Add Swagger support

   - [Program](api\Program.cs)
     - run api and open to [swagger](https://localhost:7120/swagger/index.html)

   b. Add Postman support

   - [stress-test console](stress)

   c. Attributes creation & usage

   - [Model Validation](api\Models\FibRequestDto.cs)

   d. Middleware and filter usage

   - [Default Middleware](api\Program.cs)

   e. Unit tests

   - [Test Project](tests\tests.csproj)
     - [BigIntFibServiceTests](tests\BigIntFibServiceTests.cs)
     - [IntFibServiceTests](tests\IntFibServiceTests.cs)
     - [LngFibServiceTests](tests\LngFibServiceTests.cs)

## Requirements:

Implement an API capable of generating and returning a subsequence from a sequence of Fibonacci numbers. The API should have a controller with an endpoint accepting the following parameters:

1. The index of the first number in Fibonacci sequence that starts subsequence.

   - [FibRequestDto](api\Models\FibRequestDto.cs)

2. The index of the last number in Fibonacci sequence that ends subsequence.

   - [FibRequestDto](api\Models\FibRequestDto.cs)

3. A Boolean, which indicates whether it can use cache or not.

   - [FibRequestDto](api\Models\FibRequestDto.cs)

4. A time in milliseconds for how long it can run. If generating the first number in subsequence takes longer than that time, the program should return error. Otherwise as many numbers as were generated with extra information indicating the timeout occurred.

   - Partially Implemented.
     - [FibRequestDto](api\Models\FibRequestDto.cs)
     - [FibByIndex](api\Controllers\IntFibController.cs)
     - [ListFibWithCache](api\Services\IntFibService.cs)

5. A maximum amount of memory the program can use. If, during the execution of the request this amount is reached, the execution aborts. The program should return as many generated numbers similarly to the way it does in case of timeout reached.

   - Partially Implemented.
     - [AppMonitorService](api\Services\AppMonitorService.cs)
     - [AppStateService](api\Services\AppStateService.cs)

6. The return from the endpoint should be a JSON containing the subsequence from the sequence of Fibonacci numbers that is matching the input indexes.

   - [IntFibController](api\Controllers\IntFibController.cs)
   - [FibRequestDto](api\Models\FibRequestDto.cs)
   - [AppStateDto](api\Models\AppStateDto.cs)

7. The controller that accepts requests should use async pattern.
   It should schedule the work on two background threads and wait for results asynchronously.
   The generation of Fibonacci numbers should happen on at least two background threads, where the next number in sequence should be generated on a different thread.

   - Partially Implemented
     - [IntFibController](api\Controllers\IntFibController.cs)
     - [ListFibWithCache](api\Services\IntFibService.cs)

8. Please bear in mind, there could be many requests landing simultaneously and those should use the same background threads that are executing already.

   - Did not complete BackGroundTaskQueue

9. There should be a cache for numbers, so that subsequent requests can rely on it in order to speed up the Fibonacci numbers generation.
   The cache should be invalidated after a time period, where the period is defined in configuration.

   - [AppCacheService](api\Services\AppCacheService.cs)
   - [AppMonitorService](api\Services\AppMonitorService.cs)
   - [AppStateService](api\Services\AppStateService.cs)
