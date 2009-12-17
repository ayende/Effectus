CREATE TABLE ToDoActions
(
	Id bigint identity not null primary key,
	Version int not null,
	Title nvarchar(255) not null,
	Content nvarchar(max) not null,
	Status int not null,
	CreatedAt datetime not null,
	CompleteBy datetime null
)

INSERT INTO [ToDo].[dbo].[ToDoActions] 
           ([Version], 
            [Title], 
            [Content], 
            [Status], 
            [CreatedAt], 
            [CompleteBy]) 
VALUES     (1, 
            'Write App', 
            'Should really write the application', 
            1, 
            Getdate(), 
            Getdate() + 3) 

INSERT INTO [ToDo].[dbo].[ToDoActions] 
           ([Version], 
            [Title], 
            [Content], 
            [Status], 
            [CreatedAt], 
            [CompleteBy]) 
VALUES     (1, 
            'Write Post', 
            'Writing a blog post is nice and easy', 
            0, 
            Getdate(), 
            Getdate() + 3) 

INSERT INTO [ToDo].[dbo].[ToDoActions] 
           ([Version], 
            [Title], 
            [Content], 
            [Status], 
            [CreatedAt], 
            [CompleteBy]) 
VALUES     (1, 
            'Publish Post', 
            'After writing, we should publish it', 
            0, 
            Getdate(), 
            Getdate() + 3) 

INSERT INTO [ToDo].[dbo].[ToDoActions] 
           ([Version], 
            [Title], 
            [Content], 
            [Status], 
            [CreatedAt], 
            [CompleteBy]) 
VALUES     (1, 
            'Monitor Post', 
            'It is considered nice to answer comments', 
            0, 
            Getdate(), 
            Getdate() + 3)