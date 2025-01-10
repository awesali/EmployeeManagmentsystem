const API_URL = "https://localhost:7191/api";

$(document).ready(function () {
    // Handle login form submission
    $("#login-form").submit(function (e) {
        e.preventDefault();

        const username = $("#username").val();
        const password = $("#password").val();

        // Send login request
        $.ajax({
            url: `${API_URL}/auth/login`,
            method: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify({
                username: username,
                passwordHash: password,
            }),
            success: function (response) {
                localStorage.setItem("token", response.token);
                $("#login-section").hide();
                $("#employee-section").show();
                fetchEmployees();
            },
            error: function (e) {
                console.error("Login error: ", e.responseText);
                alert("Login failed! Please check your credentials.");
            },
        });
    });

    // Fetch employees
    function fetchEmployees() {
        const token = localStorage.getItem("token");

        if (!token) {
            alert("You need to login first.");
            return;
        }

        $.ajax({
            url: `${API_URL}/Employee`,
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
                Accept: "application/json",
            },
            success: function (employees) {
                $("#Employee-list").empty();
                employees.forEach((emp) => {
                    $("#Employee-list").append(`
                        <tr>
                            <td>${emp.id}</td>
                            <td>${emp.name}</td>
                            <td>${emp.designation}</td>
                            <td>${emp.salary}</td>
                            <td>${emp.department}</td>
                            <td>
                                <button class="btn btn-warning edit-btn" data-id="${emp.id}">Edit</button>
                                <button class="btn btn-danger delete-btn" data-id="${emp.id}">Delete</button>
                            </td>
                        </tr>
                    `);
                });
            },
            error: function (e) {
                console.error("Error fetching employees: ", e.responseText);
                alert("Failed to fetch employees.");
            },
        });
    }

    // Handle employee deletion
    $(document).on("click", ".delete-btn", function () {
        const id = $(this).data("id");
        const token = localStorage.getItem("token");

        if (!token) {
            alert("Please log in first.");
            return;
        }

        if (confirm("Are you sure you want to delete this employee?")) {
            $.ajax({
                url: `${API_URL}/Employee/${id}`,
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
                success: function () {
                    alert("Employee deleted successfully!");
                    fetchEmployees();
                },
                error: function (e) {
                    console.error("Error deleting employee: ", e.responseText);
                    alert("Failed to delete employee. Please try again.");
                },
            });
        }
    });

    // Show the Add Employee form
    $("#add-employee-btn").click(function () {
        $("#form-title").text("Add Employee");
        $("#employee-form")[0].reset();
        $("#employee-form").data("id", null);
        $("#employee-section").hide();
        $("#employee-form-section").show();
    });

    // Handle Add or Edit Employee form submission
    $("#employee-form").on("submit", function (e) {
        e.preventDefault();

        const token = localStorage.getItem("token");
        const id = $(this).data("id");

        const data = {
            name: $("#name").val(),
            designation: $("#designation").val(),
            salary: parseFloat($("#salary").val()),
            department: $("#department").val(),
        };

        const method = id ? "PUT" : "POST";
        const url = id ? `${API_URL}/Employee/${id}` : `${API_URL}/Employee`;

        $.ajax({
            url: url,
            method: method,
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
            data: JSON.stringify(data),
            success: function () {
                alert(id ? "Employee updated successfully!" : "Employee added successfully!");
                $("#employee-form-section").hide();
                $("#employee-section").show();
                fetchEmployees();
            },
            error: function (e) {
                console.error("Error saving employee: ", e.responseText);
                alert("Failed to save employee. Please try again.");
            },
        });
    });

    // Show the Edit Employee form (No API call)
    $(document).on("click", ".edit-btn", function () {
        const id = $(this).data("id");

        if (!id) {
            alert("Invalid employee ID!");
            return;
        }

        // Manually populate form fields for demonstration
        $("#name").val(""); // Replace with sample data or leave blank
        $("#designation").val("");
        $("#salary").val("");
        $("#department").val("");

        // Set form ID for editing
        $("#employee-form").data("id", id);

        // Update form title and visibility
        $("#form-title").text("Edit Employee");
        $("#employee-section").hide();
        $("#employee-form-section").show();
    });
});
