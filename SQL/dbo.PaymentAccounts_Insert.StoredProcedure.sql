

CREATE proc [dbo].[PaymentAccounts_Insert]
						@VenueId int
					   ,@AccountId nvarchar(250)
					   ,@PaymentTypeId int
					   ,@CreatedBy int
					   ,@Id int OUTPUT

as

/*

	-- @VenueId (FK from Venues Table) - MISSING TABLE

					Declare	 @VenueId int = 1
							,@AccountId nvarchar(250) = 'Acct from Stripe'
							,@PaymentTypeId int = 2
							,@UserId int = 1
							,@Id int = 0

		Execute dbo.PaymentAccounts_Insert
							 @VenueId
							,@AccountId
							,@PaymentTypeId
							,@UserId
							,@Id

				Select *
				From dbo.PaymentAccounts;
*/

BEGIN

			INSERT INTO [dbo].[PaymentAccounts]
					   ([VenueId]
					   ,[AccountId]
					   ,[PaymentTypeId]
					   ,[CreatedBy]
					   ,[ModifiedBy])
				 VALUES
					   (@VenueId
					   ,@AccountId
					   ,@PaymentTypeId
					   ,@CreatedBy
					   ,@CreatedBy)

				 SET @Id = SCOPE_IDENTITY()

End


GO
