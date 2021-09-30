@RD /S /Q "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\deploy"
md "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\"
cd "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\"
dotnet publish  -c Release -o "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\deploy"
docker build -t orderservice .

@RD /S /Q "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\EcommService\deploy"
md "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\EcommService\"
cd "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\EcommService\"
dotnet publish  -c Release -o "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\EcommService\deploy"
docker build -t ecommservice .

pause