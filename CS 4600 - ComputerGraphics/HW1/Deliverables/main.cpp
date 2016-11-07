#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include <string>
#include <vector>
#include <iostream>
#include <fstream>

unsigned int g_windowWidth = 800;
unsigned int g_windowHeight = 600;
char* g_windowName = "HW1-OpenGL-Basics";
double angle = 0.0;

GLFWwindow* g_window;

// model data
std::vector<float> g_meshVertices;
std::vector<float> g_meshNormals;
std::vector<unsigned int> g_meshIndices;
std::vector<unsigned int> verticie_count;

GLfloat g_modelViewMatrix[16];

// auxiliary math functions
float dotProduct(const float* a, const float* b)
{
	return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
}

void crossProduct(const float* a, const float* b, float* r)
{
	r[0] = a[1] * b[2] - a[2] * b[1];
	r[1] = a[2] * b[0] - a[0] * b[2];
	r[2] = a[0] * b[1] - a[1] * b[0];
}

void normalize(float* a)
{
	const float len = sqrt(a[0] * a[0] + a[1] * a[1] + a[2] * a[2]);

	a[0] /= len;
	a[1] /= len;
	a[2] /= len;
}

void computeVector(const int a, const int b, float* vector){
	float x1 = g_meshVertices.at(3 * a);
	float y1 = g_meshVertices.at(3 * a + 1);
	float z1 = g_meshVertices.at(3 * a + 2);

	float x2 = g_meshVertices.at(3 * b);
	float y2 = g_meshVertices.at(3 * b + 1);
	float z2 = g_meshVertices.at(3 * b + 2);

	vector[0] = x1 - x2;
	vector[1] = y1 - y2;
	vector[2] = z1 - z2;
}


void computeNormals()
{
	size_t size = g_meshIndices.size();
	g_meshNormals.resize(size);

	// the code below sets all normals to point in the z-axis, so we get a boring constant gray color
	// the following should be replaced with your code for normal computation (Task 1)
	//
	//Modified Code by Mathhew J Howa - U0805396
	//
	verticie_count.resize(g_meshVertices.size());
	float vector1[3];
	float vector2[3];
	float normal[3];
	
	for (int v = 0; v < (int)size / 3; v++)
	{	
		int a = g_meshIndices.at(v * 3);
		int b = g_meshIndices.at(v * 3 + 1);
		int c = g_meshIndices.at(v * 3 + 2);

		computeVector(a, b, vector1);
		computeVector(a, c, vector2);

		crossProduct(vector1, vector2, normal);


		g_meshNormals[3 * a + 0] += normal[0];
		g_meshNormals[3 * a + 1] += normal[1];
		g_meshNormals[3 * a + 2] += normal[2];
		g_meshNormals[3 * b + 0] += normal[0];
		g_meshNormals[3 * b + 1] += normal[1];
		g_meshNormals[3 * b + 2] += normal[2];
		g_meshNormals[3 * c + 0] += normal[0];
		g_meshNormals[3 * c + 1] += normal[1];
		g_meshNormals[3 * c + 2] += normal[2];
	}

	//Normalize all the vectors
	for (int i = 0; i < g_meshNormals.size() / 3; i++){
		normal[0] = g_meshNormals.at(i * 3);
		normal[1] = g_meshNormals.at(i * 3 + 1);
		normal[2] = g_meshNormals.at(i * 3 + 2);
		normalize(normal);
		
		g_meshNormals.at(i * 3) = normal[0];
		g_meshNormals.at(i * 3 + 1) = normal[1];
		g_meshNormals.at(i * 3 + 2) = normal[2];
	}
}


void loadObj(std::string p_path)
{
	std::ifstream nfile;
	nfile.open(p_path);
	std::string s;

	while (nfile >> s)
	{
		if (s.compare("v") == 0)
		{
			float x, y, z;
			nfile >> x >> y >> z;
			g_meshVertices.push_back(x);
			g_meshVertices.push_back(y);
			g_meshVertices.push_back(z);
		}		
		else if (s.compare("f") == 0)
		{
			std::string sa, sb, sc;
			unsigned int a, b, c;
			nfile >> sa >> sb >> sc;

			a = std::stoi(sa);
			b = std::stoi(sb);
			c = std::stoi(sc);

			g_meshIndices.push_back(a - 1);
			g_meshIndices.push_back(b - 1);
			g_meshIndices.push_back(c - 1);
		}
		else
		{
			std::getline(nfile, s);
		}
	}

	computeNormals();

	std::cout << p_path << " loaded. Vertices: " << g_meshVertices.size() / 3 << " Triangles: " << g_meshIndices.size() / 3 << std::endl;
}

double getTime()
{
	return glfwGetTime();
}

void glfwErrorCallback(int error, const char* description)
{
	std::cerr << "GLFW Error " << error << ": " << description << std::endl;
	exit(1);
}

void glfwKeyCallback(GLFWwindow* p_window, int p_key, int p_scancode, int p_action, int p_mods)
{
	if (p_key == GLFW_KEY_ESCAPE && p_action == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(g_window, GL_TRUE);
	}
}

void initWindow()
{
	// initialize GLFW
	glfwSetErrorCallback(glfwErrorCallback);
	if (!glfwInit())
	{
		std::cerr << "GLFW Error: Could not initialize GLFW library" << std::endl;
		exit(1);
	}

	g_window = glfwCreateWindow(g_windowWidth, g_windowHeight, g_windowName, NULL, NULL);
	if (!g_window)
	{
		glfwTerminate();
		std::cerr << "GLFW Error: Could not initialize window" << std::endl;
		exit(1);
	}

	// callbacks
	glfwSetKeyCallback(g_window, glfwKeyCallback);

	// Make the window's context current
	glfwMakeContextCurrent(g_window);

	// turn on VSYNC
	glfwSwapInterval(1);
}

void initGL()
{
	glClearColor(1.f, 1.f, 1.f, 1.0f);
	
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	glEnable(GL_DEPTH_TEST);
	glShadeModel(GL_SMOOTH);

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(45.0f, (GLfloat) g_windowWidth / (GLfloat)g_windowHeight, 0.1f, 10.0f);
}

void clearModelViewMatrix()
{
	for (int i = 0; i < 4; ++i)
	{
		for (int j = 0; j < 4; ++j)
		{
			g_modelViewMatrix[4 * i + j] = 0.0f;
		}
	}
}

void updateModelViewMatrix()
{	
	clearModelViewMatrix();

	g_modelViewMatrix[0] = cos(angle);
	g_modelViewMatrix[2] = -sin(angle);
	g_modelViewMatrix[5] = 1.0f;
	g_modelViewMatrix[10] = cos(angle);
	g_modelViewMatrix[8] = sin(angle);


	//g_modelViewMatrix[0] = 1.0;
	//g_modelViewMatrix[5] = 1.0;
	//g_modelViewMatrix[10] = 1.0;

	g_modelViewMatrix[14] = -5.0f;
	g_modelViewMatrix[15] = 1.0f;

}

void setModelViewMatrix()
{
	glMatrixMode(GL_MODELVIEW);
	updateModelViewMatrix();
	glLoadMatrixf(g_modelViewMatrix);
	angle = getTime();

}

void render()
{
	setModelViewMatrix();

	glBegin(GL_TRIANGLES);

	for (size_t f = 0; f < g_meshIndices.size(); ++f)
	{
		const float scale = 0.1f;
		const unsigned int idx = g_meshIndices[f];
		const float x = scale * g_meshVertices[3 * idx + 0];
		const float y = scale * g_meshVertices[3 * idx + 1];
		const float z = scale * g_meshVertices[3 * idx + 2];

		const float nx = g_meshNormals[3 * idx + 0];
		const float ny = g_meshNormals[3 * idx + 1];
		const float nz = g_meshNormals[3 * idx + 2];
				
		glNormal3f(nx, ny, nz);
		glVertex3f(x, y, z);
	}

	glEnd();
	
}

void renderLoop()
{
	while (!glfwWindowShouldClose(g_window))
	{
		// clear buffers
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		render();

		// Swap front and back buffers
		glfwSwapBuffers(g_window);

		// Poll for and process events
		glfwPollEvents();
	}
}

int main()
{
	initWindow();
	initGL();
	loadObj("data/teapot.obj");
	renderLoop();
}
