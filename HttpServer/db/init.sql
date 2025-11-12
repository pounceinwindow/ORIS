-- =========================
-- DROP (в правильном порядке)
-- =========================
DROP TABLE IF EXISTS reviews;
DROP TABLE IF EXISTS experience_details;
DROP TABLE IF EXISTS experiences;

-- =========================
-- EXPERIENCES (каталог)
-- =========================
CREATE TABLE IF NOT EXISTS experiences (
                                           id                    SERIAL PRIMARY KEY,
                                           slug                  TEXT NOT NULL UNIQUE,
                                           title                 TEXT NOT NULL,
                                           city                  TEXT NOT NULL,
                                           category_name         TEXT NOT NULL,
                                           price_from            NUMERIC(10,2) NOT NULL,
    rating                NUMERIC(3,2),
    reviews_count         INT,
    hero_url              TEXT DEFAULT '',
    instant_confirmation  BOOLEAN DEFAULT FALSE,
    free_cancellation     BOOLEAN DEFAULT FALSE
    );

-- =========================
-- EXPERIENCE_DETAILS (детальная)
-- =========================
CREATE TABLE IF NOT EXISTS experience_details (
                                                  id               SERIAL PRIMARY KEY,
                                                  experience_id    INT NOT NULL REFERENCES experiences(id) ON DELETE CASCADE,
    category         TEXT,
    city             TEXT,
    title            TEXT,
    hero             TEXT DEFAULT '',
    hero_url         TEXT DEFAULT '',
    rating           NUMERIC(3,2),
    rating_text      TEXT,
    reviews          INT,
    languages        TEXT,
    duration         TEXT,
    price            NUMERIC(10,2),
    address          TEXT,
    meeting          TEXT,
    cancel_policy    TEXT,
    valid_until      TIMESTAMP NULL,
    description_html TEXT,
    chips_json       TEXT DEFAULT '[]',
    love_json        TEXT DEFAULT '[]',
    included_json    TEXT DEFAULT '[]',
    remember_json    TEXT DEFAULT '[]',
    more_json        TEXT DEFAULT '[]'
    );

-- =========================
-- REVIEWS
-- =========================
CREATE TABLE IF NOT EXISTS reviews (
                                       id            SERIAL PRIMARY KEY,
                                       experience_id INT NOT NULL REFERENCES experiences(id) ON DELETE CASCADE,
    author        TEXT NOT NULL,
    comment       TEXT NOT NULL,
    rating        INT NOT NULL CHECK (rating BETWEEN 1 AND 5),
    created_at    TIMESTAMP NOT NULL DEFAULT NOW()
    );

-- =========================
-- SEED: EXPERIENCES (URLs взяты из твоего варианта, который грузил картинки)
-- =========================
INSERT INTO experiences (slug, title, city, category_name, price_from, rating, reviews_count, hero_url, instant_confirmation, free_cancellation) VALUES
                                                                                                                                                     ('entrance-ticket-to-chambord-castle','Entrance ticket to Chambord Castle','Chambord','Attractions & guided tours',22.00,4.7,110,'https://images.musement.com/cover/0072/36/chateau-de-chambord-1-jpg_header-7135742.jpeg?q=50&fit=crop&auto=format&w=1680&h=600&dpr=1', TRUE, TRUE),

                                                                                                                                                     ('loire-valley-day-from-tours-with-azay-le-rideau-villandry','Loire Valley Day from Tours with Azay-le-Rideau & Villandry','Tours','Attractions & guided tours',212.00,4.8,35,'https://images.musement.com/cover/0162/95/thumb_16194779_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('e-bike-tour-to-chambord-from-villesavin','E-Bike Tour to Chambord from Villesavin','Chambord','Attractions & guided tours',200.00,4.6,18,'https://images.musement.com/cover/0163/15/thumb_16214539_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('retro-sidecar-tour-by-night','Retro sidecar tour by night','Tours','Activities',49.00,4.6,20,'https://images.musement.com/cover/0163/14/thumb_16213739_cover_header.jpg?q=50&fit=crop&auto=format&w=1680&h=600&dpr=1', TRUE, TRUE),

                                                                                                                                                     ('half-day-sidecar-tour-of-the-loire-valley-from-tours','Half-day sidecar tour of the Loire Valley from Tours','Tours','Activities',473.00,4.9,15,'https://images.musement.com/cover/0163/14/thumb_16213747_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('chenonceau-and-chambord-tour-with-wine-tasting','Chenonceau and Chambord Tour with Wine-Tasting','Tours','Attractions & guided tours',247.00,4.7,40,'https://images.musement.com/cover/0001/74/thumb_73756_cover_header.jpeg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('guided-visit-villandry-azay-le-rideau-chateaux-from-tours','Guided visit Villandry & Azay-le-Rideau châteaux from Tours','Tours','Attractions & guided tours',117.00,4.6,22,'https://images.musement.com/cover/0163/02/thumb_16201125_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('a-day-in-chambord-and-chenonceau-with-private-lunch','A day in Chambord and Chenonceau with private lunch','Tours','Attractions & guided tours',254.00,4.8,17,'https://images.musement.com/cover/0162/95/thumb_16194788_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('great-escape-sidecar-tour-from-tours','Great Escape sidecar tour from Tours','Tours','Activities',237.00,4.8,19,'https://images.musement.com/cover/0163/14/thumb_16213737_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('retro-classic-sidecar-tour-from-tours','Retro Classic sidecar tour from Tours','Tours','Activities',118.00,4.7,12,'https://images.musement.com/cover/0142/79/thumb_14178770_cover_header.jpeg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('full-day-tour-of-chambord-and-chenonceau-from-tours','Full Day Tour of Chambord and Chenonceau from Tours','Tours','Attractions & guided tours',207.00,4.8,28,'https://images.musement.com/cover/0162/95/thumb_16194790_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('entrance-ticket-for-zooparc-de-beauval','Entrance ticket for ZooParc de Beauval','Saint-Aignan','Tickets & events',49.00,4.7,300,'https://images.musement.com/cover/0165/48/thumb_16447200_cover_header.jpg?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('afternoon-wine-tour-to-vouvray','Afternoon wine tour to Vouvray','Tours','Attractions & guided tours',105.00,4.9,50,'https://images.musement.com/cover/0001/65/thumb_64023_cover_header.png?w=720&q=60', TRUE, TRUE),

                                                                                                                                                     ('e-bike-tour-to-chambord-from-tours','E-bike Tour to Chambord from Tours','Tours','Attractions & guided tours',200.00,4.6,16,'https://images.musement.com/cover/0163/15/thumb_16214542_cover_header.jpg?w=720&q=60', TRUE, TRUE);

-- =========================
-- SEED: EXPERIENCE_DETAILS (копируем hero_url в hero и hero_url)
-- =========================
INSERT INTO experience_details
(experience_id, category, city, title, hero, hero_url, rating, rating_text, reviews, languages, duration, price, cancel_policy, description_html,
 chips_json, love_json, included_json, remember_json, more_json)
SELECT
    e.id,
    e.category_name,
    e.city,
    e.title,
    e.hero_url,            -- hero
    e.hero_url,            -- hero_url
    e.rating,
    'Excellent',
    COALESCE(e.reviews_count,0),
    'en',
    'Flexible',
    e.price_from,
    'Free cancellation up to 24 hours in advance.',
    CASE e.slug
        WHEN 'entrance-ticket-to-chambord-castle'
            THEN '<p>Discover the elegance of Chambord Castle, a masterpiece of Renaissance architecture surrounded by lush greenery and a moat.</p>'
        WHEN 'loire-valley-day-from-tours-with-azay-le-rideau-villandry'
            THEN '<p>Explore the Loire Valley with a guided tour of Azay-le-Rideau and Villandry castles.</p>'
        WHEN 'e-bike-tour-to-chambord-from-villesavin'
            THEN '<p>Ride an e-bike through scenic routes to Chambord Castle, with stops at charming villages.</p>'
        WHEN 'retro-sidecar-tour-by-night'
            THEN '<p>Experience Tours by night in a vintage sidecar with a local guide.</p>'
        WHEN 'half-day-sidecar-tour-of-the-loire-valley-from-tours'
            THEN '<p>See the best of the Loire Valley in half a day aboard a retro sidecar.</p>'
        WHEN 'chenonceau-and-chambord-tour-with-wine-tasting'
            THEN '<p>Visit two iconic castles and enjoy a wine tasting in a traditional cave.</p>'
        WHEN 'guided-visit-villandry-azay-le-rideau-chateaux-from-tours'
            THEN '<p>Guided visit with transportation and picnic lunch included.</p>'
        WHEN 'a-day-in-chambord-and-chenonceau-with-private-lunch'
            THEN '<p>Full-day tour with a refined lunch in a private château room.</p>'
        WHEN 'great-escape-sidecar-tour-from-tours'
            THEN '<p>Discover the heart of the Loire Valley on a vintage sidecar.</p>'
        WHEN 'retro-classic-sidecar-tour-from-tours'
            THEN '<p>Unusual places and local stories with your sidecar guide.</p>'
        WHEN 'full-day-tour-of-chambord-and-chenonceau-from-tours'
            THEN '<p>Visit two of the most famous castles in the Loire Valley.</p>'
        WHEN 'entrance-ticket-for-zooparc-de-beauval'
            THEN '<p>Explore one of the most beautiful zoological parks in the world.</p>'
        WHEN 'afternoon-wine-tour-to-vouvray'
            THEN '<p>Become a wine expert in 4.5 hours and taste the best of Vouvray.</p>'
        WHEN 'e-bike-tour-to-chambord-from-tours'
            THEN '<p>Cycle from Tours to Chambord with electric bikes and scenic views.</p>'
        ELSE '<p>Explore the beauty of the Loire Valley with this unforgettable experience.</p>'
        END,
    '[]','[]','[]','[]','[]'   -- заполним ниже для Chambord
FROM experiences e;

-- Наполняем списки для главного продукта (как на твоём скрине)
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Explore a Renaissance icon at your own pace",
    "Climb the double-helix staircase for sweeping terrace views",
    "See royal apartments and grand halls",
    "Enjoy access to gardens and parkland"
  ]',
    included_json = '[
    "Entrance ticket to Château de Chambord",
    "Access to the château and gardens",
    "Booking fees"
  ]',
    remember_json = '[
    "Your voucher can be shown on a mobile device",
    "Some rooms and towers involve stairs",
    "Wear comfortable shoes",
    "Large bags may be subject to security checks"
  ]',
    address = 'Château de Chambord, Place Saint-Louis, 41250 Chambord, France',
    meeting = 'Present your voucher at the Château de Chambord main entrance (Place Saint-Louis, 41250 Chambord).'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'entrance-ticket-to-chambord-castle';


-- =========================
-- REVIEWS SEED
-- =========================
INSERT INTO reviews (experience_id, author, comment, rating, created_at) VALUES
                                                                             ((SELECT id FROM experiences WHERE slug = 'entrance-ticket-to-chambord-castle'),'Anna','Great castle, skip-the-line really saved time.',5, NOW() - INTERVAL '5 days'),
                                                                             ((SELECT id FROM experiences WHERE slug = 'entrance-ticket-to-chambord-castle'),'Lucas','Very impressive place, audio guide recommended.',4, NOW() - INTERVAL '12 days'),
                                                                             ((SELECT id FROM experiences WHERE slug = 'loire-valley-day-from-tours-with-azay-le-rideau-villandry'),'Maria','Perfect day trip, guide was amazing.',5, NOW() - INTERVAL '8 days'),
                                                                             ((SELECT id FROM experiences WHERE slug = 'e-bike-tour-to-chambord-from-villesavin'),'John','E-bikes were fun and easy to use.',5, NOW() - INTERVAL '3 days');
/* =========================
   PATCH: наполнение деталей из Musement
   ========================= */

-- 1) Loire Valley day from Tours with visit to Azay-le-Rideau & Villandry
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Explore the interior of the Azay-le-Rideau chateau",
    "Discover the elegant gardens of Villandry chateau",
    "Visit a winery and taste Vouvray appellation wines"
  ]',
    included_json = '[
    "Guided tour",
    "Wine tasting",
    "Transportation in air-conditioned vehicle",
    "Skip-the-line tickets"
  ]',
    remember_json = '[
    "Children under 4 years old are not allowed",
    "A ticket is strictly required for every participant",
    "Tour requires a minimum of 2 participants"
  ]',
    address       = 'Office de Tourisme & des Congrès Tours Loire Valley, Rue Bernard Palissy, Tours, France',
    meeting       = 'Present your voucher at the Tourist Office of Tours 10 minutes before departure time scheduled.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'loire-valley-day-from-tours-with-azay-le-rideau-villandry';

-- 2) E-Bike Tour to Chambord from Villesavin
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Explore the Château de Chambord, jewel of the Renaissance",
    "Visit the Château de Villesavin, a charming family property",
    "Ride ~25 km through the lush Forest of Chambord",
    "Taste local specialties"
  ]',
    included_json = '[
    "Guided tour",
    "Transportation in air-conditioned vehicle",
    "Skip-the-line tickets",
    "Food tasting",
    "E-bike"
  ]',
    remember_json = '[
    "Not suitable for people under 155 cm",
    "Children under 4 years old are not allowed",
    "Be used to riding a bike and in good physical condition",
    "Wear suitable clothing and comfortable sport shoes",
    "In case of heavy rain, the bike tour may be replaced by a minibus tour"
  ]',
    address       = 'Château de Villesavin, Villesavin, Tour-en-Sologne, France',
    meeting       = 'Present your voucher to your guide 10 minutes before departure time at Château de Villesavin.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'e-bike-tour-to-chambord-from-villesavin';

-- 3) E-bike Tour to Chambord from Tours
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Visit the Château de Chambord, a UNESCO Heritage Site",
    "Discover the Château de Villesavin, a charming family property",
    "Taste local specialties",
    "Enjoy a 25 km ride through the Forest of Chambord"
  ]',
    included_json = '[
    "Guided tour",
    "Helmet",
    "Transportation in air-conditioned vehicle",
    "Skip-the-line tickets",
    "Food tasting",
    "E-bike"
  ]',
    remember_json = '[
    "Not recommended for people under 155 cm",
    "Not accessible for children under 10 years old",
    "You must be used to riding a bike and in good physical condition",
    "Snack includes 1 glass of wine (min age 18); inform allergies in advance",
    "Recommended to wear suitable clothing and comfortable sports shoes",
    "In case of heavy rain, tour may be replaced by a minibus"
  ]',
    address       = 'Office de Tourisme & des Congrès Tours Loire Valley, Rue Bernard Palissy, Tours, France',
    meeting       = 'Present your voucher to your guide 10 minutes before departure time in front of Tours Tourist Office.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'e-bike-tour-to-chambord-from-tours';

-- 4) Full Day Tour of Chambord and Chenonceau from Tours
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Visit the Château de Chenonceau and its gardens",
    "Enjoy a picnic in a wonderful park (lunch not included)",
    "Explore the impressive royal Château de Chambord"
  ]',
    included_json = '[
    "Entrance fees",
    "Guided tour",
    "Transportation in air-conditioned vehicle"
  ]',
    remember_json = '[
    "A ticket is required for all participants including children and infants",
    "Not accessible for children under 4 years old"
  ]',
    address       = 'Office de Tourisme & des Congrès Tours Loire Valley, Rue Bernard Palissy, Tours, France',
    meeting       = 'Present your voucher to your guide 10 minutes before departure time in front of Tours Tourist Office.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'full-day-tour-of-chambord-and-chenonceau-from-tours';

-- 5) Retro sidecar tour by night
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Discover Tours by night with a gentleman local sider",
    "Enjoy the \\\"tourangelle\\\" atmosphere in a vintage sidecar",
    "Stop for a delicious wine tasting break under a starry sky"
  ]',
    included_json = '[
    "Local rider / guide",
    "Vintage sidecar",
    "Private night tour",
    "Wine tasting break"
  ]',
    remember_json = '[
    "Price is per sidecar with 1 or 2 passengers",
    "You can book up to 2 sidecars per type"
  ]',
    address       = 'Rue Bernard Palissy 78-82, 37000 Tours, France',
    meeting       = 'Tourist office – Office de Tourisme Tours Val de Loire.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 5 days before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'retro-sidecar-tour-by-night';

-- 6) Great Escape sidecar tour from Tours
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Explore the heart of the Loire Valley with a local sider",
    "Admire the landscape on board a vintage sidecar",
    "Enjoy the ride between vineyards and castles"
  ]',
    included_json = '[
    "Local rider / guide",
    "Vintage sidecar",
    "Private tour"
  ]',
    remember_json = '[
    "Price is per sidecar with 1 or 2 passengers",
    "You can book up to 2 sidecars per type"
  ]',
    address       = 'Tourist office – Office de Tourisme Tours Val de Loire, 78–82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'Meet your local guide at the entrance of the tourist office.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 5 days before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'great-escape-sidecar-tour-from-tours';

-- 7) Retro Classic sidecar tour from Tours
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Discover Tours with a gentleman local sider",
    "Take a step back in time in a vintage sidecar",
    "Enjoy a ride between vineyards and castles"
  ]',
    included_json = '[
    "Local rider / guide",
    "Vintage sidecar",
    "Private tour"
  ]',
    remember_json = '[
    "Price is per sidecar with 1 or 2 passengers",
    "You can book up to 2 sidecars per type"
  ]',
    address       = 'Tourist office – Office de Tourisme Tours Val de Loire, 78–82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'Meet your local guide at the entrance of the tourist office.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 5 days before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'retro-classic-sidecar-tour-from-tours';

-- 8) Half-day sidecar tour of the Loire Valley from Tours
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Discover the best of the Loire Valley in half a day",
    "Enjoy the landscape on board a vintage sidecar",
    "Visit the Loire castles with a local sider"
  ]',
    included_json = '[
    "Local rider / guide",
    "Vintage sidecar",
    "Private custom tour"
  ]',
    remember_json = '[
    "Price is per sidecar with 1 or 2 passengers",
    "You can book up to 2 sidecars per type"
  ]',
    address       = 'Tourist office – Office de Tourisme Tours Val de Loire, 78–82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'Meet your local guide at the entrance of the tourist office.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 5 days before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'half-day-sidecar-tour-of-the-loire-valley-from-tours';
/* =========================
   PATCH #2 — fill details for 5 experiences
   ========================= */

-- 1) Chenonceau & Chambord + wine tasting
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Visit the two most popular Loire Valley castles",
    "Explore the countryside and become part of the beautiful landscape",
    "Dive into the world of winemaking — see the transformation from grape to glass"
  ]',
    included_json = '[
    "Entrance tickets to châteaux de Chenonceau and Chambord",
    "Transportation in 8-seat minibus",
    "Guide",
    "Guided tour",
    "Wine tasting"
  ]',
    remember_json = '[
    "Mention pickup point during booking: Tours 8:55 AM or Amboise 9:25 AM",
    "Wear comfortable shoes",
    "Bring baby seat(s) if joining with children/infants"
  ]',
    address       = 'Tourist Office of Tours, 78-82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'Meet your guide in front of the Tourist Office of Tours at 8:55 AM or in front of the Tourist Office of Amboise at 9:25 AM.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'chenonceau-and-chambord-tour-with-wine-tasting';

-- 2) Guided visit Villandry & Azay-le-Rideau from Tours
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Experience for real the château daily life of a French family",
    "Discover the elegant Villandry gardens",
    "Enjoy interesting commentary by a local guide"
  ]',
    included_json = '[
    "Châteaux entrance fees",
    "Friendly and professional local guide",
    "Transfers by minivan"
  ]',
    remember_json = '[
    "Not accessible for children under 4 years old",
    "A ticket is strictly required for every participant (incl. infants/pets)",
    "Tour requires a minimum of 2 participants"
  ]',
    address       = 'Tours Tourist Office, 78-82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'Present your voucher at the Tourist Office of Tours 10 minutes before departure time scheduled.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'guided-visit-villandry-azay-le-rideau-chateaux-from-tours';

-- 3) ZooParc de Beauval — entrance ticket
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Skip the line"]',
    love_json     = '[
    "Visit one of the most beautiful zoological parks in the world",
    "See nearly 35,000 animals including two giant pandas",
    "Learn about the life and habits of the species living in the zoo"
  ]',
    included_json = '["Entrance ticket"]',
    remember_json = '[
    "Up to 15 catering places for lunch",
    "Stroller rental, lockers, picnic area and car park available",
    "Free admission for children under 3 years old"
  ]',
    address       = 'ZooParc de Beauval, 41110, Saint-Aignan-sur-Cher, France',
    meeting       = 'Please present your barcode ticket at the entrance of the Zoo.',
    cancel_policy = 'Unfortunately, we can’t offer you a refund nor can we change or cancel your booking for this product, due to our partner’s policy.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'entrance-ticket-for-zooparc-de-beauval';

-- 4) Afternoon wine tour to Vouvray
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Visit a local winery in the Vouvray region",
    "Learn about the winemaking process from the wine to the glass",
    "Get tips to recognize wines like a pro",
    "Explore the famous underground caves"
  ]',
    included_json = '[
    "Friendly and professional guide",
    "Transfers by minivan",
    "Wine tasting",
    "Winery tasting",
    "Winery tour"
  ]',
    remember_json = '[
    "Must be 18 years of age to drink alcohol",
    "Returns to original departure point",
    "A ticket is strictly required for every participant; last-minute additions not accepted",
    "Not accessible for children under 4 years old"
  ]',
    address       = 'Meet your guide in front of Tours Tourist Office, 78-82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'In front of the Tourist Office of Tours.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'afternoon-wine-tour-to-vouvray';

-- 5) A day in Chambord & Chenonceau with private lunch
UPDATE experience_details d
SET
    chips_json    = '["Instant confirmation","Mobile voucher accepted","Free cancellation"]',
    love_json     = '[
    "Enjoy a delicious local lunch at a lovely private castle",
    "Discover the Château de Chenonceau",
    "Visit Château de Chambord with an expert guide"
  ]',
    included_json = '[
    "Entrance to the castles",
    "Traditional lunch with local wines and aperitif",
    "Friendly and professional local guide",
    "Transfers by minivan"
  ]',
    remember_json = '[
    "Not accessible for children under 4 years old",
    "Minimum drinking age is 18 years",
    "Ticket strictly required for all participants (incl. infants/pets)",
    "Dietary restrictions must be reported no more than 48 hours before departure"
  ]',
    address       = 'Tours Tourist Office, 78-82 Rue Bernard Palissy, 37000 Tours, France',
    meeting       = 'Present your voucher to your guide 10 minutes before departure time in front of Tours Tourist Office.',
    cancel_policy = 'Receive a 100% refund if you cancel up to 24 hours before the experience begins.'
    FROM experiences e
WHERE d.experience_id = e.id
  AND e.slug = 'a-day-in-chambord-and-chenonceau-with-private-lunch';
