
CREATE TABLE IF NOT EXISTS experiences (
                                           id            SERIAL PRIMARY KEY,
                                           slug          TEXT NOT NULL UNIQUE,
                                           title         TEXT NOT NULL,
                                           city          TEXT NOT NULL,
                                           category_name TEXT NOT NULL,
                                           price_from    NUMERIC(10,2) NOT NULL,
    rating        NUMERIC(3,2),
    reviews_count INT,
    hero_url      TEXT
    );

CREATE TABLE IF NOT EXISTS experience_details (
                                                  id              SERIAL PRIMARY KEY,
                                                  experience_id   INT NOT NULL REFERENCES experiences(id) ON DELETE CASCADE,
    category        TEXT,
    city            TEXT,
    title           TEXT,
    hero            TEXT,
    rating          NUMERIC(3,2),
    rating_text     TEXT,
    reviews         INT,
    languages       TEXT,
    duration        TEXT,
    price           NUMERIC(10,2),
    address         TEXT,
    meeting         TEXT,
    cancel_policy   TEXT,
    description_html TEXT,
    chips_json      JSONB,
    love_json       JSONB,
    included_json   JSONB,
    remember_json   JSONB
    );

CREATE TABLE IF NOT EXISTS reviews (
                                       id            SERIAL PRIMARY KEY,
                                       experience_id INT NOT NULL REFERENCES experiences(id) ON DELETE CASCADE,
    author        TEXT NOT NULL,
    comment       TEXT NOT NULL,
    rating        INT  NOT NULL,
    created_at    TIMESTAMP NOT NULL DEFAULT NOW()
    );


TRUNCATE TABLE reviews RESTART IDENTITY CASCADE;
TRUNCATE TABLE experience_details RESTART IDENTITY CASCADE;
TRUNCATE TABLE experiences RESTART IDENTITY CASCADE;


INSERT INTO experiences (slug, title, city, category_name, price_from, rating, reviews_count, hero_url) VALUES
                                                                                                            ('entrance-ticket-to-chambord-castle',
                                                                                                             'Entrance ticket to Chambord Castle',
                                                                                                             'Chambord',
                                                                                                             'Attractions & guided tours',
                                                                                                             22.00, 4.7, 110,
                                                                                                             'https://images.musement.com/cover/0072/36/chateau-de-chambord-1-jpg_header-7135742.jpeg?w=720&q=60'),

                                                                                                            ('loire-valley-day-from-tours-with-azay-le-rideau-villandry',
                                                                                                             'Loire Valley Day from Tours with Azay-le-Rideau & Villandry',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             212.00, 4.8, 35,
                                                                                                             'https://images.musement.com/cover/0162/95/thumb_16194779_cover_header.jpg?w=720&q=60'),

                                                                                                            ('e-bike-tour-to-chambord-from-villesavin',
                                                                                                             'E-Bike Tour to Chambord from Villesavin',
                                                                                                             'Chambord',
                                                                                                             'Attractions & guided tours',
                                                                                                             200.00, 4.6, 18,
                                                                                                             'https://images.musement.com/cover/0163/15/thumb_16214539_cover_header.jpg?w=720&q=60'),

                                                                                                            ('retro-sidecar-tour-by-night',
                                                                                                             'Retro sidecar tour by night',
                                                                                                             'Tours',
                                                                                                             'Activities',
                                                                                                             49.00, 4.6, 20,
                                                                                                             'https://images.musement.com/cover/0142/79/thumb_14178772_cover_header.jpeg?w=720&q=60'),

                                                                                                            ('half-day-sidecar-tour-of-the-loire-valley-from-tours',
                                                                                                             'Half-day sidecar tour of the Loire Valley from Tours',
                                                                                                             'Tours',
                                                                                                             'Activities',
                                                                                                             473.00, 4.9, 15,
                                                                                                             'https://images.musement.com/cover/0163/14/thumb_16213747_cover_header.jpg?w=720&q=60'),

                                                                                                            ('chenonceau-and-chambord-tour-with-wine-tasting',
                                                                                                             'Chenonceau and Chambord Tour with Wine-Tasting',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             247.00, 4.7, 40,
                                                                                                             'https://images.musement.com/cover/0001/74/thumb_73756_cover_header.jpeg?w=720&q=60'),

                                                                                                            ('guided-visit-villandry-azay-le-rideau-chateaux-from-tours',
                                                                                                             'Guided visit Villandry & Azay-le-Rideau châteaux from Tours',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             117.00, 4.6, 22,
                                                                                                             'https://images.musement.com/cover/0163/02/thumb_16201125_cover_header.jpg?w=720&q=60'),

                                                                                                            ('a-day-in-chambord-and-chenonceau-with-private-lunch',
                                                                                                             'A day in Chambord and Chenonceau with private lunch',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             254.00, 4.8, 17,
                                                                                                             'https://images.musement.com/cover/0162/95/thumb_16194788_cover_header.jpg?w=720&q=60'),

                                                                                                            ('great-escape-sidecar-tour-from-tours',
                                                                                                             'Great Escape sidecar tour from Tours',
                                                                                                             'Tours',
                                                                                                             'Activities',
                                                                                                             237.00, 4.8, 19,
                                                                                                             'https://images.musement.com/cover/0163/14/thumb_16213737_cover_header.jpg?w=720&q=60'),

                                                                                                            ('retro-classic-sidecar-tour-from-tours',
                                                                                                             'Retro Classic sidecar tour from Tours',
                                                                                                             'Tours',
                                                                                                             'Activities',
                                                                                                             118.00, 4.7, 12,
                                                                                                             'https://images.musement.com/cover/0142/79/thumb_14178770_cover_header.jpeg?w=720&q=60'),

                                                                                                            ('full-day-tour-of-chambord-and-chenonceau-from-tours',
                                                                                                             'Full Day Tour of Chambord and Chenonceau from Tours',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             207.00, 4.8, 28,
                                                                                                             'https://images.musement.com/cover/0162/95/thumb_16194790_cover_header.jpg?w=720&q=60'),

                                                                                                            ('entrance-ticket-for-zooparc-de-beauval',
                                                                                                             'Entrance ticket for ZooParc de Beauval',
                                                                                                             'Saint-Aignan',
                                                                                                             'Tickets & events',
                                                                                                             49.00, 4.7, 300,
                                                                                                             'https://images.musement.com/cover/0165/48/thumb_16447200_cover_header.jpg?w=720&q=60'),

                                                                                                            ('afternoon-wine-tour-to-vouvray',
                                                                                                             'Afternoon wine tour to Vouvray',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             105.00, 4.9, 50,
                                                                                                             'https://images.musement.com/cover/0001/65/thumb_64023_cover_header.png?w=720&q=60'),

                                                                                                            ('e-bike-tour-to-chambord-from-tours',
                                                                                                             'E-bike Tour to Chambord from Tours',
                                                                                                             'Tours',
                                                                                                             'Attractions & guided tours',
                                                                                                             200.00, 4.6, 16,
                                                                                                             'https://images.musement.com/cover/0163/15/thumb_16214542_cover_header.jpg?w=720&q=60');


INSERT INTO experience_details
(experience_id, category, city, title, hero, rating, rating_text, reviews, languages, duration, price, cancel_policy)
SELECT
    e.id,
    e.category_name,
    e.city,
    e.title,
    e.hero_url,
    e.rating,
    'Excellent',
    COALESCE(e.reviews_count, 0),
    'en',
    'Flexible',
    e.price_from,
    'Free cancellation up to 24 hours before.'
FROM experiences e;


INSERT INTO reviews (experience_id, author, comment, rating, created_at) VALUES
                                                                             ((SELECT id FROM experiences WHERE slug = 'entrance-ticket-to-chambord-castle'),
                                                                              'Anna', 'Great castle, skip-the-line really saved time.', 5, NOW() - INTERVAL '5 days'),

                                                                             ((SELECT id FROM experiences WHERE slug = 'entrance-ticket-to-chambord-castle'),
                                                                              'Lucas', 'Very impressive place, audio guide recommended.', 4, NOW() - INTERVAL '12 days'),

                                                                             ((SELECT id FROM experiences WHERE slug = 'loire-valley-day-from-tours-with-azay-le-rideau-villandry'),
                                                                              'Maria', 'Perfect day trip, guide was amazing.', 5, NOW() - INTERVAL '8 days'),

                                                                             ((SELECT id FROM experiences WHERE slug = 'e-bike-tour-to-chambord-from-villesavin'),
                                                                              'John', 'E-bikes were fun and easy to use.', 5, NOW() - INTERVAL '3 days');
