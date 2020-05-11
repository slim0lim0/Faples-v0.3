#include "Physics.hpp"
#include "Time.hpp"
#include "Map.hpp"
#include "View.hpp"
#include <cmath>
#include <iostream>
#include <algorithm>

namespace fpl
{
	// physics variables
	double const down_jump_multiplier = 0.35355339;
	double const epsilon = 0.00001;
	double const fall_speed = 670;
	double const float_coefficient = 0.01;
	double const float_drag_1 = 100000;
	double const float_drag_2 = 10000;
	double const float_multiplier = 0.0008928571428571428;
	double const fly_force = 120000;
	double const fly_jump_dec = 0.35;
	double const fly_speed = 200;
	double const gravity_acc = 2000;
	double const jump_speed = 555;
	double const max_friction = 2;
	double const max_land_speed = 162.5;
	double const min_friction = 0.05;
	double const shoe_fly_acc = 0;
	double const shoe_fly_speed = 0;
	double const shoe_mass = 100;
	double const shoe_swim_acc = 1;
	double const shoe_swim_speed_h = 1;
	double const shoe_swim_speed_v = 1;
	double const shoe_walk_acc = 1;
	double const shoe_walk_drag = 1;
	double const shoe_walk_jump = 1.0;
	double const shoe_walk_slant = 0.9;
	double const shoe_walk_speed = 1.0;
	double const slip_force = 60000;
	double const slip_speed = 120;
	double const swim_force = 120000;
	double const swim_jump = 700;
	double const swim_speed = 140;
	double const swim_speed_dec = 0.9;
	double const walk_drag = 80000;
	double const walk_force = 140000;
	double const walk_speed = 150;
	double const ladder_room = 10;
	double const fall_timeout = 50;

	const std::string FOOT = "0";
	const std::string CLIMB = "1";
	const std::string SEAT = "2";

	Physics::Physics()
	{
	}

	Physics::~Physics()
	{
	}

	void Physics::Reset(int nx, int ny, int nwidth, int nheight)
	{
		x = nx;
		y = ny;
		width = nwidth;
		height = nheight;

		LEFT = false;
		RIGHT = false;
		UP = false;
		DOWN = false;

		FLYING = false;

		fh = nullptr;
		djump = nullptr;

		fallTimeOut = 0;
		fallDuration = 0;
	}

	void Physics::Jump()
	{
		if (fh) {		
			CalculateJump();
		}
	}

	void Physics::Update()
	{
		bool m_left = LEFT && !RIGHT;
		bool m_right = RIGHT && !LEFT;
		bool m_up = UP && !DOWN;
		bool m_down = DOWN && !UP;

		if (fallTimeOut > 0)
			fallTimeOut -= time::delta;

		if (fallTimeOut < 0)
			fallTimeOut = 0;

		CalculateNextFoothold();
		
		while (time::delta_total > laststep)
		{
			laststep += 0.01;
			double delta = 0.01;

			tx = floorf(x + width / 2);
			ty = floorf(y + height);

			if (fh)
			{
				if (fh->type == FOOT)
				{
					CalculateWalk(delta);
				}
				else if (fh->type == CLIMB)
				{
					CalculateClimb(delta);
				}
			}
			else
			{
				CalculateFall(delta);
			}

			//Default physics
			double nnx = x + (vx * delta);
			double nny = y + (vy * delta);

			px = x + (width / 2);
			py = y + height;

			x = nnx;

			int finalY = miny - height;

			int finalxRight = minxRight - width;
			int finalxLeft = minxLeft;

			bool firstCaseHandled = false;

			if (ch != NULL)
			{
				if (UP)
				{
					bool bHoldValid = true;

					if (fh)
					{
						if (fh->type == FOOT)
						{
							if (ch->prev != NULL || ch->next != NULL)
							{
								bHoldValid = false;
							}
						}
					}

					if (bHoldValid)
					{
						if (RIGHT)
						{
							if (nnx <= ch->x1)
							{
								nnx = ch->x1 - (width / 2);
								SetFoothold(ch);
								ch = nullptr;
								djump = nullptr;
								vx = 0;
								vy = 0;
							}
						}
						else if (LEFT)
						{
							if (nnx >= ch->x1)
							{
								nnx = ch->x1 - (width / 2);
								SetFoothold(ch);
								ch = nullptr;
								djump = nullptr;
								vx = 0;
								vy = 0;
							}
						}
						else
						{
							nnx = ch->x1 - (width/2);
							SetFoothold(ch);
							ch = nullptr;
							djump = nullptr;
							vx = 0;
							vy = 0;
						}
					}
				}
				else if (DOWN)
				{
					if (fh != NULL)
					{
						if (fh->type == FOOT)
						{
							nny += 10;
							finalY = nny;
						}
					}

					if (RIGHT)
					{
						if (nnx <= ch->x1)
						{
							nnx = ch->x1 - (width / 2);
							SetFoothold(ch);
							ch = nullptr;
							djump = nullptr;
							vx = 0;
							vy = 0;
						}
					}
					else if (LEFT)
					{
						if (nnx >= ch->x1)
						{
							nnx = ch->x1 - (width / 2);
							SetFoothold(ch);
							ch = nullptr;
							djump = nullptr;
							vx = 0;
							vy = 0;
						}
					}
					else
					{
						nnx = ch->x1 - (width / 2);
						SetFoothold(ch);
						ch = nullptr;
						djump = nullptr;
						vx = 0;
						vy = 0;
					}

					CalculateNextFoothold();
				}	
			}
			

			if (nnx <= finalxRight)
				x = nnx;
			else if(vx > 0)
			{
				firstCaseHandled = true;
				x = finalxRight;
				vx = 0;
			}

			if (!firstCaseHandled)
			{
				if (nnx >= finalxLeft)
					x = nnx;
				else if (vx < 0)
				{
					x = finalxLeft;
					vx = 0;
				}
			}
							
			if (nny <= finalY)
				y = nny;
			else
			{
				y = finalY - 1;
				finalY = y;

				SetFoothold(nextfh);
				nextfh = nullptr;
				if (fh)
				{
					layer = fh->layer;
					vy = 0;
					djump = nullptr;
					ch = nullptr;
				}
			}				

			tx = x + (width / 2);
			ty = y + height;

			delta = 0;
		}
	}
	

	
	void Physics::CalculateNextFoothold()
	{
		bool m_left = LEFT && !RIGHT;
		bool m_right = RIGHT && !LEFT;
		bool m_up = UP && !DOWN;
		bool m_down = DOWN && !UP;

		tx = x + width / 2;
		ty = y + height;

	
		miny = View::bottom;

		minxRight = View::right;
		minxLeft = View::left;

		nextfh = nullptr;
		ch = nullptr;

		for (auto & nearfh : footholds) {
			// Check that nearest foothold is not the current one
			if (&nearfh != fh && &nearfh != djump)
			{
				// Check that the nearest foothold is type foot
				if (nearfh.type == FOOT)
				{
					// Check to see if player is colliding with a wall
					if (nearfh.x1 == nearfh.x2)
					{
						if (px < nearfh.x1 - 1)
						{
							if (nearfh.y1 < nearfh.y2)
							{
								if (py >= nearfh.y1 && py <= nearfh.y2)
								{
									if(nearfh.x1 < minxRight)
										minxRight = nearfh.x1;
								}
							}
							else
							{
								if (py <= nearfh.y1 && py >= nearfh.y2)
								{
									if (nearfh.x1 < minxRight)
										minxRight = nearfh.x1;
								}
							}
						}
						else if(px > nearfh.x1 + 1)
						{
							if (nearfh.y1 < nearfh.y2)
							{
								if (py >= nearfh.y1 && py <= nearfh.y2)
								{
									if (nearfh.x1 > minxLeft)
										minxLeft = nearfh.x1;
								}
							}
							else
							{
								if (py <= nearfh.y1 && py >= nearfh.y2)
								{
									if (nearfh.x1 > minxLeft)
										minxLeft = nearfh.x1;
								}
							}
						}
					}
					else
					{
						// Check that the player is within the x bounds of the foothold
						if (tx >= nearfh.x1-1 && tx <= nearfh.x2+1)
						{
							// Calculate player distance on y, handles slopes

							double maxDisX = nearfh.x2 - nearfh.x1;
							double currDisX = tx - nearfh.x1;

							double percX = currDisX / maxDisX;

							int maxDisY = 0;
							int currDisY = 0;

							if (nearfh.y1 <= nearfh.y2)
							{
								maxDisY = nearfh.y2 - nearfh.y1;
								currDisY = ceilf(maxDisY * percX);

								if (nearfh.y1 + currDisY >= ty)
								{
									// Take the minimum y value found
									if (nearfh.y1 + currDisY <= miny)
									{
										miny = nearfh.y1 + currDisY - 2;
										nextx = nearfh.x1;
										nextfh = &nearfh;
									}
								}
							}
							else
							{
								maxDisY = abs(nearfh.y2 - nearfh.y1);
								currDisY = ceilf(maxDisY * percX);

								if (nearfh.y1 - currDisY >= ty)
								{
									// Take the minimum y value found
									if (nearfh.y1 - currDisY <= miny)
									{
										miny = nearfh.y1 - currDisY - 2;
										nextfh = &nearfh;
									}
								}
							}
						}
					}
				}
				// Check that the nearest foothold is type climb
				else if (nearfh.type == CLIMB)
				{
					int climbty = y + height;
					// Calculate by middle of player
					int maxDisY = 0;
					int currDisY = 0;

					if (nearfh.y1 <= nearfh.y2)
					{
						if (climbty >= nearfh.y1 && climbty <= nearfh.y2)
						{
							if (nearfh.next != NULL)
							{
								double maxDisY = nearfh.y2 - nearfh.y1;
								double currDisY = climbty - nearfh.y1;

								double percY = currDisY / maxDisY;

								int maxDisX = abs(nearfh.x2 - nearfh.x1);
								int currDisX = ceilf(maxDisX * percY);


								if (tx <= (nearfh.x1 + ladder_room) && tx >= nearfh.x1 - ladder_room)
								{
									ch = &nearfh;
								}
							}
						}
					}
					else if(nearfh.y1 >= nearfh.y2)
					{
						if (nearfh.next != NULL)
						{
							if (climbty <= nearfh.y1 && climbty >= nearfh.y2)
							{
								double maxDisY = nearfh.y1 - nearfh.y2;
								double currDisY = climbty - nearfh.y2;

								double percY = currDisY / maxDisY;

								int maxDisX = abs(nearfh.x2 - nearfh.x1);
								int currDisX = ceilf(maxDisX * percY);


								if (tx <= (nearfh.x1 + ladder_room) && tx >= nearfh.x1 - ladder_room)
								{
									ch = &nearfh;
								}
							}
						}
				
					}
				}
			}
		}
	}

	void Physics::ClearFoothold(bool refreshTimeout)
	{
		fh = nullptr;

		if (refreshTimeout)
			fallTimeOut = fall_timeout;
		else
			fallTimeOut = 0;
	}

	void Physics::SetFoothold(Foothold* newfh)
	{
		fh = newfh;
		fallDuration = 0;
	}

	void Physics::CalculateJump()
	{
		// TODO: Weaker jump from rope 
		if (fh->type == CLIMB && (LEFT || RIGHT))
		{
			vy = shoe_walk_jump * (jump_speed - shoe_mass) * (FLYING ? -0.7 : -1);
			double fx = fh->x2 - fh->x1;
			double fy = fh->y2 - fh->y1;
			double fmax = walk_speed * shoe_walk_speed;

			if (LEFT)
				vx = std::max(std::min(vx, -fmax * 0.8), -fmax);
			else if (RIGHT)
				vx = std::min(std::max(vx, fmax * 0.8), fmax);
			else
				vx = vx;

			ClearFoothold(false);
		}
		else if (fh->type == FOOT)
		{
			if (DOWN )
			{
				if (!fh->cantDrop)
				{
					vx = 0;
					vy = -jump_speed * down_jump_multiplier;
					djump = fh;
					ClearFoothold(false);
				}
			}
			else
			{
				vy = shoe_walk_jump * jump_speed * (FLYING ? -0.7 : -1);
				double fx = fh->x2 - fh->x1, fy = fh->y2 - fh->y1, fmax = walk_speed * shoe_walk_speed;
				(LEFT && fy < 0) || (RIGHT && fy > 0) ? fmax *= (1 + (fy * fy) / (fx * fx + fy * fy))
					: 0;
				vx = LEFT ? std::max(std::min(vx, -fmax * 0.8), -fmax)
					: RIGHT ? std::min(std::max(vx, fmax * 0.8), fmax) : vx;

				ClearFoothold(false);
			}
		}
	}

	void Physics::CalculateWalk(double delta)
	{
		bool m_left = LEFT && !RIGHT;
		bool m_right = RIGHT && !LEFT;
		bool m_up = UP && !DOWN;
		bool m_down = DOWN && !UP;

		double const fx = fh->x2 - fh->x1, fy = fh->y2 - fh->y1, fx2 = fx * fx, fy2 = fy * fy,
			len = sqrt(fx2 + fy2);
		double mvr = vx * len / fx; 

		mvr -= fh->force;
		double fs = (map::current["info"]["fs"] ? map::current["info"]["fs"] : 1.) / shoe_mass
			* delta;
		double maxf = (FLYING ? swim_speed_dec : 1.) * walk_speed * shoe_walk_speed;
		double drag = std::max(std::min(shoe_walk_drag, max_friction), min_friction)
			* walk_drag;
		double slip = fy / len;
		if (shoe_walk_slant < std::abs(slip)) {
			double slipf = slip_force * slip;
			double slips = slip_speed * slip;
			mvr += m_left ? -drag * fs : m_right ? drag * fs : 0;
			mvr = slips > 0 ? std::min(slips, mvr + slipf * delta)
				: std::max(slips, mvr + slipf * delta);
		}
		else {
			mvr = m_left
				? mvr < -maxf ? std::min(-maxf, mvr + drag * fs)
				: std::max(-maxf, mvr - shoe_walk_acc * walk_force * fs)
				: m_right
				? mvr > maxf ? std::max(maxf, mvr - drag * fs)
				: std::min(maxf, mvr + shoe_walk_acc * walk_force * fs)
				: mvr < 0. ? std::min(0., mvr + drag * fs)
				: mvr > 0. ? std::max(0., mvr - drag * fs) : mvr;
		}
		mvr += fh->force;
		vx = mvr * fx / len,
			vy = mvr * fy / len;

		if (m_right)
		{
			if (tx >= fh->x2)
			{
				if (fh->next == NULL)
				{
					ClearFoothold(true);
				}
				else
				{
					if (fh->next->x2 != fh->next->x1)
						SetFoothold(Foothold::getById(fh->nextid, fh->group, fh->layer));
					else
					{
						vx = 0;
						tx = fh->x2 - 1;
					}
				}
			}
		}
		else if (m_left)
		{
			if (tx <= fh->x1)
			{
				if (fh->prev == NULL)
				{
					ClearFoothold(true);
				}
				else
				{
					if (fh->next->x2 != fh->next->x1)
						SetFoothold(Foothold::getById(fh->previd, fh->group, fh->layer));
					else
					{
						vx = 0;
						tx = fh->x2 + 1;
					}
				}
			}
		}	
	}

	void Physics::CalculateFall(double delta)
	{
		bool m_left = LEFT && !RIGHT;
		bool m_right = RIGHT && !LEFT;
		bool m_up = UP && !DOWN;
		bool m_down = DOWN && !UP;

		double shoefloat = float_drag_2 / shoe_mass * delta;
		vy > 0 ? vy = std::max(0., vy - shoefloat) : vy = std::min(0., vy + shoefloat);
		vy = std::fmin(vy + gravity_acc * delta, fall_speed);
		vx = m_left
			? vx > -float_drag_2 * float_multiplier
			? std::fmax(-float_drag_2 * float_multiplier, vx - 2 * shoefloat)
			: vx
			: m_right
			? vx < float_drag_2 * float_multiplier
			? std::fmin(float_drag_2 * float_multiplier, vx + 2 * shoefloat)
			: vx
			: vy < fall_speed
			? vx > 0 ? std::fmax(0., vx - float_coefficient * shoefloat)
			: std::fmin(0., vx + float_coefficient * shoefloat)
			: vx > 0 ? std::fmax(0., vx - shoefloat)
			: std::fmin(0., vx + shoefloat);

		fallDuration += 1;
	}

	void Physics::CalculateClimb(double delta)
	{
		bool m_left = LEFT && !RIGHT;
		bool m_right = RIGHT && !LEFT;
		bool m_up = UP && !DOWN;
		bool m_down = DOWN && !UP;

		double const fx = fh->x2 - fh->x1, fy = fh->y2 - fh->y1, fx2 = fx * fx, fy2 = fy * fy,
			len = sqrt(fx2 + fy2);
		double mvr = vy * len / fy;

		double fs = (map::current["info"]["fs"] ? map::current["info"]["fs"] : 1.) / shoe_mass
			* delta;
		double maxf = (FLYING ? swim_speed_dec : 1.) * walk_speed * shoe_walk_speed;
		double drag = std::max(std::min(shoe_walk_drag, max_friction), min_friction)
			* walk_drag;

		if (m_up)
		{
			if (mvr < -maxf)
				mvr = std::fmin(-maxf, mvr + drag * fs);
			else
				mvr = std::fmax(-maxf, mvr - shoe_walk_acc * walk_force * fs);
		}
		else if (m_down)
		{
			if (mvr > maxf)
				mvr = std::fmax(maxf, mvr - drag * fs);
			else
				mvr = std::fmin(maxf, mvr + shoe_walk_acc * walk_force * fs);
		}
		else
		{
			if (mvr < 0.)
				mvr = std::fmin(0., mvr + drag * fs);
			else if (mvr > 0.)
				mvr = std::fmax(0., mvr - drag * fs);
			else
				mvr = mvr;
		}

			// Declutter mvr calculation
		/*mvr = m_up
			? mvr < -maxf ? std::fmin(-maxf, mvr + drag * fs)
			: std::fmax(-maxf, mvr - shoe_walk_acc * walk_force * fs)
			: m_down
			? mvr > maxf ? std::fmax(maxf, mvr - drag * fs)
			: std::fmin(maxf, mvr + shoe_walk_acc * walk_force * fs)
			: mvr < 0. ? std::fmin(0., mvr + drag * fs)
			: mvr > 0. ? std::fmax(0., mvr - drag * fs) : mvr;*/

		mvr += fh->force;
		vx = mvr * fx / len,
		vy = mvr * fy / len;	


		if (m_up)
		{
			if (fh->y1 < fh->y2)
			{
				if (ty <= fh->y1 - 1)
				{
					if (fh->prev == NULL)
					{
						ty = fh->y1 + 1;
						vy = 0;
					}
					else
					{

						SetFoothold(Foothold::getById(fh->previd, fh->group, fh->layer));
					}
				}
			}
			else
			{
				if (ty <= fh->y2 - 1)
				{
					if (fh->next == NULL)
					{
						ty = fh->y2 + 1;
						vy = 0;
					}
					else
					{
						SetFoothold(Foothold::getById(fh->nextid, fh->group, fh->layer));
					}
				}
			}		
		}
		else if (m_down)
		{
			if (fh->y1 < fh->y2)
			{
				if (ty >= fh->y2 + 1)
				{
					if (fh->next == NULL)
					{
						ClearFoothold(false);
					}
					else
					{
						SetFoothold(Foothold::getById(fh->nextid, fh->group, fh->layer));
					}
				}
			}
			else
			{
				if (fh->next == NULL)
				{
					ClearFoothold(false);
				}
				else
				{
					if (ty >= fh->y1 + 1)
					{
						if (fh->prev == NULL)
						{
							ClearFoothold(false);
						}
						else
						{
							fh = Foothold::getById(fh->previd, fh->group, fh->layer);
						}
					}
				}
			}
		}
	}


	vector2i Physics::GetPosition()
	{
		return vector2i(x, y);
	}
	int Physics::GetWalkSpeed(int entitySpeed)
	{
		return walk_speed + entitySpeed;
	}
	int Physics::GetFallSpeed()
	{
		return fall_speed;
	}
}