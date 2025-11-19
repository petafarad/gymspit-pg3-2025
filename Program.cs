static bool AddValue(string value, string[] data, int count)
{
    if (count >= data.Length)
    {
        Console.WriteLine("I'm afraid I can't do that.");
        return false;
    }

    data[count] = value;
    return true;
}

static bool RemoveValue(string[] data, int index, int count)
{
    if (index < 0 || index >= count)
    {
        Console.WriteLine("I'm afraid I can't do that.");
        return false;
    }

    for (int i = index; i < count - 1; i += 1)
    {
        data[i] = data[i + 1];
    }
    data[count - 1] = "";
    return true;
}


static void AddUser(string username, string[] users, ref int userCount)
{
    int index = Array.IndexOf(users, username);
    if (index >= 0)
    {
        Console.WriteLine("User already exists.");
        return;
    }

    if (AddValue(username, users, userCount))
    {
        userCount += 1;
    }
}

static void RemoveUser(string username, string[] users, ref int userCount)
{
    int index = Array.IndexOf(users, username);
    if (index < 0)
    {
        Console.WriteLine("User does not exist.");
        return;
    }

    if (index >= 0 && RemoveValue(users, index, userCount))
    {
        userCount -= 1;
    }
}


static void AddPost(string post, string author, string[] posts, string[] postAuthors, ref int postCount)
{
    // Přidá post a paralelně autora do postAuthors.
    // Pokud není místo, vypíše zprávu (AddValue to ošetří).
    if (postCount >= posts.Length)
    {
        Console.WriteLine("I'm afraid I can't do that.");
        return;
    }

    // Přidáme nejprve obsah, pak autora; při selhání autora vrátíme zpět obsah.
    bool addedPost = AddValue(post, posts, postCount);
    if (!addedPost)
    {
        return;
    }

    bool addedAuthor = AddValue(author, postAuthors, postCount);
    if (!addedAuthor)
    {
        // rollback - odstranit právě přidaný post na indexu postCount
        RemoveValue(posts, postCount, postCount + 1);
        return;
    }

    postCount += 1;
}

static string[] GetUserPosts(string user, string[] posts, string[] postAuthors, int postCount)
{
    // Spočítat kolik postů daného uživatele existuje
    int found = 0;
    for (int i = 0; i < postCount; i += 1)
    {
        if (postAuthors[i] == user)
        {
            found += 1;
        }
    }

    if (found == 0)
    {
        return new string[] { };
    }

    string[] result = new string[found];
    int idx = 0;
    for (int i = 0; i < postCount; i += 1)
    {
        if (postAuthors[i] == user)
        {
            result[idx++] = posts[i];
        }
    }

    return result;
}


static void AddFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
{
    // Neumožňovat self-follow
    if (follower == followee)
    {
        Console.WriteLine("Cannot follow yourself.");
        return;
    }

    // Zkontrolovat duplicitu
    for (int i = 0; i < followCount; i += 1)
    {
        if (followers[i] == follower && followees[i] == followee)
        {
            Console.WriteLine("Already following.");
            return;
        }
    }

    if (followCount >= followers.Length)
    {
        Console.WriteLine("I'm afraid I can't do that.");
        return;
    }

    if (AddValue(follower, followers, followCount) && AddValue(followee, followees, followCount))
    {
        followCount += 1;
    }
    else
    {
        // rollback při selhání druhého AddValue
        if (Array.IndexOf(followers, follower, 0) == followCount)
        {
            RemoveValue(followers, followCount, followCount + 1);
        }
    }
}

static void RemoveFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
{
    int index = -1;
    for (int i = 0; i < followCount; i += 1)
    {
        if (followers[i] == follower && followees[i] == followee)
        {
            index = i;
            break;
        }
    }

    if (index < 0)
    {
        Console.WriteLine("Follow relationship does not exist.");
        return;
    }

    if (RemoveValue(followers, index, followCount) && RemoveValue(followees, index, followCount))
    {
        followCount -= 1;
    }
}

static string[] GetUserFollows(string user, string[] followers, string[] followees, int followCount)
{
    // Vrátí seznam uživatelů, které 'user' sleduje (followees).
    int found = 0;
    for (int i = 0; i < followCount; i += 1)
    {
        if (followers[i] == user)
        {
            found += 1;
        }
    }

    if (found == 0) return new string[] { };

    string[] result = new string[found];
    int idx = 0;
    for (int i = 0; i < followCount; i += 1)
    {
        if (followers[i] == user)
        {
            result[idx++] = followees[i];
        }
    }

    return result;
}

static string[] GetUserFollowers(string user, string[] followers, string[] followees, int followCount)
{
    // Vrátí seznam uživatelů, kteří sledují 'user' (followers).
    int found = 0;
    for (int i = 0; i < followCount; i += 1)
    {
        if (followees[i] == user)
        {
            found += 1;
        }
    }

    if (found == 0) return new string[] { };

    string[] result = new string[found];
    int idx = 0;
    for (int i = 0; i < followCount; i += 1)
    {
        if (followees[i] == user)
        {
            result[idx++] = followers[i];
        }
    }

    return result;
}


// Bonus
static string[] GetUserTimeline(string user, string[] posts, string[] postAuthors, int postCount, string[] followers, string[] followees, int followCount)
{
    // Timeline obsahuje vlastní posty uživatele a posty uživatelů, které sleduje.
    // Nejnovější posty jsou vráceny první (reverzní pořadí).
    string[] following = GetUserFollows(user, followers, followees, followCount);

    // Pomocné: zjistit kolik postů bude v timeline
    int found = 0;
    for (int i = 0; i < postCount; i += 1)
    {
        if (postAuthors[i] == user)
        {
            found += 1;
            continue;
        }
        for (int j = 0; j < following.Length; j += 1)
        {
            if (postAuthors[i] == following[j])
            {
                found += 1;
                break;
            }
        }
    }

    if (found == 0) return new string[] { };

    string[] timeline = new string[found];
    int idx = 0;
    // Přidáme od nejnovějšího (poslední index je nejnovější)
    for (int i = postCount - 1; i >= 0; i -= 1)
    {
        bool include = false;
        if (postAuthors[i] == user) include = true;
        else
        {
            for (int j = 0; j < following.Length; j += 1)
            {
                if (postAuthors[i] == following[j])
                {
                    include = true;
                    break;
                }
            }
        }

        if (include)
        {
            timeline[idx++] = posts[i];
            if (idx >= found) break;
        }
    }

    return timeline;
}


int MAX_USERS = 100;
int MAX_POSTS = MAX_USERS * 100;
int MAX_FOLLOWS = MAX_USERS * (MAX_USERS + 1) / 2;

string[] users = new string[MAX_USERS];
int userCount = 0;

string[] posts = new string[MAX_POSTS];
string[] postAuthors = new string[MAX_POSTS];
int postCount = 0;

string[] followers = new string[MAX_FOLLOWS];
string[] followees = new string[MAX_FOLLOWS];
int followCount = 0;

// --- Ukázkové volání funkcí ---
AddUser("wormik", users, ref userCount);
AddUser("alice", users, ref userCount);
AddUser("bob", users, ref userCount);
AddUser("charlie", users, ref userCount);

Console.Writeline("Users: ");
PrintStrings(Slice(users, userCount));

AddPost("Hello, world!", "wormik", posts, postAuthors, ref postCount);
AddPost("My second post", "wormik", posts, postAuthors, ref postCount);
AddPost("Alice's first post", "alice", posts, postAuthors, ref postCount);
AddPost("Bob says hi", "bob", posts, postAuthors, ref postCount);
AddPost("Charlie here", "charlie", posts, postAuthors, ref postCount);

Console.Write line("Posts by wormik: ");
Console.Write line(GetUserPosts("wormik", posts, postAuthors, postCount));
Console.Write line("Posts by bob: ");

Console.Writeline("Posts by alice: ");
Console.Writeline(GetUserPosts("alice", posts, postAuthors, postCount));

// Follows
AddFollow("alice", "wormik", followers, followees, ref followCount);
AddFollow("alice", "bob", followers, followees, ref followCount);
AddFollow("bob", "wormik", followers, followees, ref followCount);

Console.Writeline("alice follows: ");
PrintStrings(GetUserFollows("alice", followers, followees, followCount));

Console.Writeline("wormik followers: ");
PrintStrings(GetUserFollowers("wormik", followers, followees, followCount));

// Timeline pro alice (měla by obsahovat vlastní posty + posty uživatelů které sleduje)
Console.Writeline("alice timeline: ");
PrintStrings(GetUserTimeline("alice", posts, postAuthors, postCount, followers, followees, followCount));

// Odebrat follow a znovu vypsat
RemoveFollow("alice", "bob", followers, followees, ref followCount);
Console.Writeline("alice follows po odstranění bob: ");
PrintStrings(GetUserFollows("alice", followers, followees, followCount));