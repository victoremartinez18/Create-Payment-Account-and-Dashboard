
CREATE proc [dbo].[PaymentAccounts_SelectAll]
									@PageIndex int
								   ,@PageSize int

as

/*
						Declare	 @PageIndex int = 0
								,@PageSize int = 10

		Execute dbo.[PaymentAccounts_SelectAll]
											@PageIndex
										   ,@PageSize
*/

BEGIN
		Declare @offset int = @PageIndex * @PageSize

			  SELECT p.[Id]
					,[VenueId]
					,v.Name
					,[AccountId]
					,[PaymentTypeId]
					,pt.Name as PaymentType
					,p.[DateCreated]
					,[DateModifed]
					,dbo.fn_GetUserJSON(p.CreatedBy) as [CreatedBy]
					,dbo.fn_GetUserJSON(p.ModifiedBy) as [ModifiedBy]
					,TotalCount = COUNT(1) OVER()
				FROM [dbo].[PaymentAccounts] as p join dbo.PaymentType as pt
						on p.PaymentTypeId = pt.Id join dbo.Venues as v
							on v.id = p.VenueId
				ORDER BY p.Id

		OFFSET @offSet Rows
		Fetch Next @PageSize Rows ONLY
End


GO
