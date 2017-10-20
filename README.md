# Code challenge for IT company
"Please create a class library with classes and unit tests that will handle the basic user login process for any application. The user should be able to register and then log in with an email and password. The registration should persist through application restarts. No front end code is needed and enough unit testing should be done to ensure everything works."

# Clarification
- Instead of using ORM such as Entity Framework or nHibernate I decided to use plain SQL commands because I find it more important to show to you.
- I also decided to use Castle Windosor in integration tests just to show that I am capable of using this tool. However I would not do so in production. See for instance: https://stackoverflow.com/questions/32594803/using-di-container-in-unit-tests

# Resources used for cryptographic part of code:
- https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
- https://stackoverflow.com/questions/14643735/how-to-generate-a-unique-token-which-expires-after-24-hours
