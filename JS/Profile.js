projectBtn = document.getElementById('projectBtn');
reviewBtn = document.getElementById('reviewsBtn');
editProfile = document.getElementById('editProfileBtn');


//Tabs
Projects = document.getElementById('projects');
Reviews = document.getElementById('reviews');
EditProfile = document.getElementById('editProfile');

projectBtn.addEventListener('click', () => {
    Projects.classList.remove('hideTab');
    projectBtn.classList.add('active');
    reviewBtn.classList.remove('active');
    editProfile.classList.remove('active');

});

reviewBtn.addEventListener('click', () => {
    Reviews.classList.remove('hideTab');
    reviewBtn.classList.add('active');
    projectBtn.classList.remove('active');
    editProfile.classList.remove('active');
});

editProfile.addEventListener('click', () => {
    EditProfile.classList.remove('hideTab');
    editProfile.classList.add('active');
    projectBtn.classList.remove('active');
    reviewBtn.classList.remove('active');
});
