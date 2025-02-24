CREATE TABLE [dbo].[UserBookHistory] (
    [UserId]       NVARCHAR (128)      NOT NULL,
    [BookId]       INT      NOT NULL,
    [DateTaken]    DATETIME NULL,
    [DateReturned] DATETIME NULL,
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[Book] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].AspNetUsers ([Id])
);

