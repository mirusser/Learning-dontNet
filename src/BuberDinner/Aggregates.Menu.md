# Domain Models

## Menu

```csharp
class Menu 
{
	Menu Create();
	void AddDinner(Dinner dinner);
	void RemoveDinner(Dinner dinner);
	void UpdateSection(MenuSection section);
}
```

```json
{
	"id": "000-000",
	"name": "Menu Name",
	"description": "",
	"averageRating": 4.5,
	"sections": [
		{
			"id": "000-000",
			"name": "Appetizers",
			"description" : "Starters":
			"items": [
				{
					"id": "000-000",
					"name": "Fried Pickles",
					"description": "Deep fried pickles",
					"price": 5.99
				}
			]
		}
	],
	"createdDateTime": "2020-01-01T00:00.0000000Z",
	"updatedDateTime": "2020-01-01T00:00.0000000Z",
	"hostId": "000-000",
	"dinnerIds": [
		"000-000",
	],
	"menuReviewIds" : [
		"000-000"
	]
}
```