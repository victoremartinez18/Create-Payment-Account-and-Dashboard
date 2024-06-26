
CREATE proc [dbo].[PaymentAccounts_Select_ById]
						@Id int

as

/*
						Declare	 @Id int = 44

		Execute dbo.PaymentAccounts_Select_ById
											@Id
*/

BEGIN

			  SELECT p.[Id]
					,[VenueId]
					,v.Name
					,[AccountId]
					,pt.Id as PaymentTypeId
					,pt.Name as PaymentType
					,p.[DateCreated]
					,[DateModifed]
					,dbo.fn_GetUserJSON(p.CreatedBy) as [CreatedBy]
					,dbo.fn_GetUserJSON(p.ModifiedBy) as [ModifiedBy]
				FROM [dbo].[PaymentAccounts] as p join dbo.PaymentType as pt
						on p.PaymentTypeId = pt.Id join dbo.Venues as v
							on v.id = p.VenueId
				Where p.Id = @Id;


End


GO
