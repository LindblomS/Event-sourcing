CREATE TABLE [dbo].[event_store](
	[version] [int] NOT NULL,
	[aggregate_id] [nvarchar](250) NOT NULL,
	[aggregate_name] [nvarchar](250) NOT NULL,
	[name] [nvarchar](250) NOT NULL,
	[data] [nvarchar](max) NOT NULL,
	[created] [datetime2](7) NOT NULL,
    CONSTRAINT pk_event_store primary key ([version], [aggregate_id]) 
) 


